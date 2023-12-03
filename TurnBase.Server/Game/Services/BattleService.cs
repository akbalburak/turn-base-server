using TurnBase.Server.Game.Battle.Core;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Services
{
    public static class BattleService
    {
        private static ReaderWriterLockSlim _rwls = new ReaderWriterLockSlim();
        private static List<IBattleItem> _battles = new List<IBattleItem>();

        public static IBattleItem CreateALevel(IBattleUser[] users,
            int stageIndex,
            int levelIndex)
        {
            // WE LOOK FOR THE LEVEL.
            BattleLevelData levelData = BattleLevelService.GetLevelData(stageIndex, levelIndex);
            if (levelData == null)
                return null;

            // WE CREATE A BATTLE.
            IBattleItem battle = new BattleItem(users, levelData);
            battle.OnDisposed += OnBattleDiposed;

            // WE ASSIGN ALL THE PLAYERS SAME BATTLE.
            foreach (IBattleUser user in users)
                user.SocketUser.SetBattle(battle);

            // WE ADD BATTLE INTO LIST.
            _rwls.EnterWriteLock();
            _battles.Add(battle);
            _rwls.ExitWriteLock();

            return battle;
        }

        private static void OnBattleDiposed(IBattleItem battleItem)
        {
            _rwls.EnterWriteLock();
            _battles.Remove(battleItem);
            _rwls.ExitWriteLock();
        }
    }
}
