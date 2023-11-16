using TurnBase.DTOLayer.Enums;
using TurnBase.Server.Battle;
using TurnBase.Server.Battle.Core;
using TurnBase.Server.Battle.Models;

namespace TurnBase.Server.Services
{
    public static class BattleService
    {
        private static ReaderWriterLockSlim _rwls = new ReaderWriterLockSlim();
        private static List<BattleItem> _battles = new List<BattleItem>();

        public static void CreateALevel(BattleUser[] users,
            int stageIndex,
            int levelIndex,
            LevelDifficulities difficulity)
        {
            // WE LOOK FOR THE LEVEL.
            BattleLevelData levelData = BattleLevelService.GetLevelData(stageIndex,levelIndex);
            if (levelData == null)
                return;

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
        }

        private static void OnBattleDiposed(BattleItem battleItem)
        {
            _rwls.EnterWriteLock();
            _battles.Remove(battleItem);
            _rwls.ExitWriteLock();
        }
    }
}
