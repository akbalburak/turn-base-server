using TurnBase.Server.Battle.Core;
using TurnBase.Server.Battle.Models;

namespace TurnBase.Server.Battle.Services
{
    public static class BattleService
    {
        private static ReaderWriterLockSlim _rwls = new ReaderWriterLockSlim();
        private static List<BattleItem> _battles = new List<BattleItem>();

        public static void CreateALevel(BattleUser[] users, int stageIndex, int levelIndex)
        {
            BattleLevelData levelData = BattleLevelService.GetLevelData($"Level_{stageIndex}_{levelIndex}");
            if (levelData == null)
                return;

            BattleItem battle = new BattleItem(users, levelData, Enums.BattleLevels.Normal);
            battle.OnDisposed += OnBattleDiposed;

            foreach (BattleUser user in users)
                user.SocketUser.SetBattle(battle);

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
