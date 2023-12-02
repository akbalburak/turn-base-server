using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Models;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public void FinalizeTurn()
        {
            // WE FIRST CHECK THE GAME END.
            CheckGameEnd();

            // WE CHECK IF THE GAME IS OVER.
            if (_gameOver)
                return;

            // IF NOT OVER WE CAN SKIP TO NEXT TURN.
            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }

        private void CheckGameEnd()
        {
            int team1AliveUnitCount = _allUnits.Count(x => !x.IsDeath && x.UnitData.TeamIndex == 1);
            int team2AliveUnitCount = _allUnits.Count(x => !x.IsDeath && x.UnitData.TeamIndex == 2);

            // IF THERE IS AN ALIVE UNIT FOR BOTH TEAM..
            if (team1AliveUnitCount > 0 && team2AliveUnitCount > 0)
                return;

            // WE WILL CHECK IF THIS IS THE LAST WAVE.

            // WHEN TWO TEAM LOSES ALL THEIR UNITS.
            if (team1AliveUnitCount == 0 && team2AliveUnitCount == 0)
            {
                BattleEndDTO drawData = new BattleEndDTO(BattleEndSates.Lose);
                SendToAllUsers(BattleActions.BattleEnd, drawData);
                Dispose();
                return;
            }

            // IF ALL PLAYERS DIED FINALIZE THE GAME.
            if (Array.TrueForAll(_users, x => x.IsDeath))
            {
                BattleEndDTO drawData = new BattleEndDTO(BattleEndSates.Lose);
                SendToAllUsers(BattleActions.BattleEnd, drawData);
                Dispose();
                return;
            }

            // MEANS ONE OF THE TEAMS IS DEFEATED.
            if (team1AliveUnitCount > 0)
            {
                foreach (BattleUser user in _users)
                {
                    // TEAM 1 WON.
                    BattleEndDTO team1EndData = new BattleEndDTO(BattleEndSates.Win);
                    team1EndData.WinnerTeam = 1;

                    if (user.IsFirstCompletion)
                        team1EndData.FirstCompletionRewards = _difficulityData.FirstCompletionRewards;

                    SendToAllUsers(BattleActions.BattleEnd, team1EndData);

                    CompleteCampaign(user.SocketUser, userId: user.SocketUser.User.Id);
                }

                Dispose();
                return;
            }

            // MEANS ONE OF THE TEAMS IS DEFEATED.
            if (team2AliveUnitCount > 0)
            {
                // TEAM 2 WON.
                BattleEndDTO team1EndData = new BattleEndDTO(BattleEndSates.Win)
                {
                    WinnerTeam = 2
                };
                SendToAllUsers(BattleActions.BattleEnd, team1EndData);
                Dispose();
                return;
            }
        }

    }
}
