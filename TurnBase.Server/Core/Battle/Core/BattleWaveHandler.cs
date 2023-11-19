using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Core
{
    public partial class BattleItem
    {
        public void FinalizeTurn()
        {
            // WE FIRST CHECK THE GAME END.
            CheckWaveEnd();

            // WE CHECK IF THE GAME IS OVER.
            if (_gameOver)
                return;

            // IF NOT OVER WE CAN SKIP TO NEXT TURN.
            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }

        private void StartWave(int waveIndex)
        {
            // WE REMOVE OLDER WAVE UNITS.
            if (_currentWave != null)
            {
                _allUnits.RemoveAll(y => _currentWave.Units.Contains(y));
                _turnHandler.RemoveUnits(_currentWave.Units);
            }

            // WE ASSIGN THE NEW UNITS.
            _currentWave = _waves[waveIndex];
            _allUnits.AddRange(_currentWave.Units);

            // WE TELL ALL THE PLAYERS THE NEW WAVE STARTED.
            SendToAllUsers(BattleActions.NewWaveStarted, new BattleWaveChangeDTO(waveIndex));

            _turnHandler.AddUnits(_currentWave.Units);
        }
        private void CheckWaveEnd()
        {
            int team1AliveUnitCount = _allUnits.Count(x => !x.IsDeath && x.TeamIndex == 1);
            int team2AliveUnitCount = _allUnits.Count(x => !x.IsDeath && x.TeamIndex == 2);

            // IF THERE IS AN ALIVE UNIT FOR BOTH TEAM..
            if (team1AliveUnitCount > 0 && team2AliveUnitCount > 0)
                return;

            // WE WILL CHECK IF THIS IS THE LAST WAVE.
            int waveIndex = Array.IndexOf(_waves, _currentWave);
            bool isLastWave = waveIndex >= _waves.Length - 1;

            // WHEN TWO TEAM LOSES ALL THEIR UNITS.
            if (team1AliveUnitCount == 0 && team2AliveUnitCount == 0)
            {
                BattleEndDTO drawData = new BattleEndDTO(BattleEndSates.Lose);
                SendToAllUsers(BattleActions.BattleEnd, drawData);
                Dispose();
                return;
            }

            // IF NOT THE LAST WAVE WE WE START A NEW WAVE.
            if (!isLastWave)
            {
                // IF ALL PLAYERS DIED FINALIZE THE GAME.
                if (Array.TrueForAll(_users, x => x.IsDeath))
                {
                    BattleEndDTO drawData = new BattleEndDTO(BattleEndSates.Lose);
                    SendToAllUsers(BattleActions.BattleEnd, drawData);
                    Dispose();
                    return;
                }

                StartWave(waveIndex + 1);
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
