using TurnBase.Server.Enums;
using TurnBase.Server.Game.Battle.Core;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Map;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Services
{
    public static class BattleService
    {
        private static ReaderWriterLockSlim _rwls = new ReaderWriterLockSlim();
        private static List<IBattleItem> _battles = new List<IBattleItem>();

        private static ReaderWriterLockSlim _rwlsUser = new ReaderWriterLockSlim();
        private static List<IBattleUser> _battleUsers = new List<IBattleUser>();

        public static IBattleItem CreateALevel(IBattleUser[] users,
            int stageIndex,
            int levelIndex)
        {
            // WE LOOK FOR THE LEVEL.
            MapDataJson levelData = BattleLevelService.GetLevelData(stageIndex, levelIndex);
            if (levelData == null)
                return null;

            // WE CREATE A BATTLE.
            IBattleItem battle = new BattleItem(users, levelData);
            battle.OnDisposed += OnBattleDiposed;

            // WE ASSIGN ALL THE PLAYERS SAME BATTLE.
            _rwlsUser.EnterWriteLock();
            foreach (IBattleUser user in users)
            {
                user.SocketUser.SetBattle(battle);
                _battleUsers.Add(user);
            }
            _rwlsUser.ExitWriteLock();

            // WE ADD BATTLE INTO LIST.
            _rwls.EnterWriteLock();
            _battles.Add(battle);
            _rwls.ExitWriteLock();

            // WE TELL ALL THE PLAYERS GAME STARTED.
            SocketResponse battleStartedData = SocketResponse.GetSuccess(ActionTypes.BattleStarting, null);
            foreach (IBattleUser user in users)
                user.SocketUser.SendToClient(battleStartedData);

            return battle;
        }

        private static void OnBattleDiposed(IBattleItem battleItem)
        {
            _rwls.EnterWriteLock();
            _battles.Remove(battleItem);
            _rwls.ExitWriteLock();

            IBattleUser[] users = battleItem.Users;

            // WE REMOVE ALL USERS FROM BATTLE LIST.
            _rwlsUser.EnterWriteLock();
            foreach (IBattleUser user in users)
                _battleUsers.Remove(user);
            _rwlsUser.ExitWriteLock();
        }

        public static IBattleItem GetBattle(long userId)
        {
            // WE SEARCH FOR THE USER IF IN BATTLE.
            _rwlsUser.EnterReadLock();
            IBattleUser user = _battleUsers.Find(x => x.SocketUser.User.Id == userId);
            _rwlsUser.ExitReadLock();

            if (user == null)
                return null;

            return user.UnitData.BattleItem;
        }
    }
}
