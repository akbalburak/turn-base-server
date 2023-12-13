using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public void FinalizeTurn()
        {
            // WE FIRST CHECK THE GAME END.
            CheckGameEnd();

            // WE CHECK IF THE GAME IS OVER.
            if (GameOver)
                return;

            // IF NOT OVER WE CAN SKIP TO NEXT TURN.
            _turnHandler.SkipToNextTurn();
            TryPlayAITurn();
        }

        private void CheckGameEnd()
        {
            int team1AliveUnitCount = _allUnits.Count(x => !x.IsDeath && x.UnitData.TeamIndex == 1);
            int team2AliveUnitCount = _allUnits.Count(x => !x.IsDeath && x.UnitData.TeamIndex == 2);

            // IF THERE IS AN ALIVE UNIT FOR BOTH TEAM..
            if (team1AliveUnitCount > 0 && team2AliveUnitCount > 0)
                return;

            // WHEN TWO TEAM LOSES ALL THEIR UNITS.
            if (team1AliveUnitCount == 0 && team2AliveUnitCount == 0)
            {
                BattleEndDTO drawData = new BattleEndDTO(BattleEndSates.Lose);
                SendToAllUsers(BattleActions.BattleEnd, drawData);
                return;
            }

            // IF ALL PLAYERS DIED FINALIZE THE GAME.
            if (Array.TrueForAll(_users, x => x.IsDeath))
            {
                BattleEndDTO drawData = new BattleEndDTO(BattleEndSates.Lose);
                SendToAllUsers(BattleActions.BattleEnd, drawData);
                return;
            }

            // MEANS TEAM 1 IS DEFEATED.
            if (team1AliveUnitCount > 0)
            {
                BattleEndRewardDTO[] completionRewards = _levelData.IFirstCompletionRewards
                    .Select(x => new BattleEndRewardDTO(x))
                    .ToArray();

                foreach (IBattleUser user in _users)
                {
                    // TEAM 1 WON.
                    BattleEndDTO team1EndData = new BattleEndDTO(BattleEndSates.Win);
                    team1EndData.WinnerTeam = 1;

                    if (user.IsFirstCompletion)
                    {
                        team1EndData.FirstCompletionRewards = completionRewards;
                    }

                    SendToAllUsers(BattleActions.BattleEnd, team1EndData);
                }

                FinalizeBattle(isVictory: true);

                return;
            }

            // MEANS TEAM 2 IS DEFEATED.
            if (team2AliveUnitCount > 0)
            {
                // TEAM 2 WON.
                BattleEndDTO team1EndData = new BattleEndDTO(BattleEndSates.Win)
                {
                    WinnerTeam = 2
                };
                SendToAllUsers(BattleActions.BattleEnd, team1EndData);
                return;
            }
        }

    }
}
