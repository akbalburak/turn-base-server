using TurnBase.Server.Core.Battle.Core;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Enums;

namespace TurnBase.Server.Core.Services
{
    public static class BattleService
    {
        private static ReaderWriterLockSlim _rwls = new ReaderWriterLockSlim();
        private static List<IBattleItem> _battles = new List<IBattleItem>();

        public static IBattleItem CreateALevel(BattleUser[] users,
            int stageIndex,
            int levelIndex,
            LevelDifficulities difficulity)
        {
            // WE LOOK FOR THE LEVEL.
            BattleLevelData levelData = BattleLevelService.GetLevelData(stageIndex, levelIndex);
            if (levelData == null)
                return null;

            // WE CREATE A BATTLE.
            BattleItem battle = new BattleItem(users, levelData, difficulity);
            battle.OnDisposed += OnBattleDiposed;

            // WE ASSIGN ALL THE PLAYERS SAME BATTLE.
            foreach (BattleUser user in users)
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
