﻿using TurnBase.DTOLayer.Enums;
using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Core.Controllers;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.Core
{
    public class BattleItem : IDisposable
    {
        public bool IsDisposed => _disposed;
        public Action<BattleItem> OnDisposed;

        private bool _gameStarted;
        private bool _gameOver;
        private bool _winnerTeam;

        private BattleLevelData _levelData;

        private BattleDifficulityData _difficulityData;
        private LevelDifficulities _difficulity;

        private BattleTurnHandler _turnHandler;

        private BattleUser[] _users;

        private BattleWave[] _waves;
        private BattleWave _currentWave;

        private List<BattleUnit> _allUnits;

        private int _skillIdCounter;
        private int _unitIdCounter;
        private int _dataIdCounter;

        private bool _disposed;

        public BattleItem(BattleUser[] users,
            BattleLevelData levelData,
            LevelDifficulities difficulity)
        {
            _allUnits = new List<BattleUnit>();

            _users = users;

            _levelData = levelData;
            _difficulity = difficulity;
            _difficulityData = levelData.GetDifficulityData(difficulity);

            _waves = _difficulityData.Waves.ToArray();
            _currentWave = _waves[0];

            // WE CREATE IDS FOR UNITS.
            foreach (BattleWave wave in _waves)
            {
                foreach (BattleNpcUnit unit in wave.Units)
                {
                    unit.SetId(++_unitIdCounter);
                    unit.SetTeam(2);
                }
            }

            // WE CREATE IDS FOR USERS.
            foreach (BattleUser user in _users)
            {
                user.SetId(++_unitIdCounter);
                user.AddSkill(new BattleDoubleSlashSkill(++_skillIdCounter, this, user));
                user.SetTeam(1);
            }

            // WE COMBINE ALL THE UNITS.
            _allUnits.AddRange(_currentWave.Units);
            _allUnits.AddRange(_users);

            _turnHandler = new BattleTurnHandler(this, users, _currentWave.Units);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _gameOver = true;

            OnDisposed?.Invoke(this);
        }

        public void ExecuteAction(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            // IF THE GAME IS OVER NO LONGER ACTIONS CAN BE EXECUTED.
            if (_gameOver)
                return;

            switch (requestData.BattleAction)
            {
                case BattleActions.LoadAll:
                    SendAllReqDataToClient(socketUser, requestData);
                    break;
                case BattleActions.IamReady:
                    StartGame();
                    break;
                case BattleActions.TurnUpdated:
                    break;
                case BattleActions.UnitBasicAttack:
                    PlayerBasicAttack(socketUser, requestData);
                    break;
                case BattleActions.UnitUseSkill:
                    PlayerUseSkill(socketUser, requestData);
                    break;
            }
        }
        private void StartGame()
        {
            _gameStarted = true;

            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }
        private void PlayerUseSkill(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleSkillUseDTO useData = requestData.GetRequestData<BattleSkillUseDTO>();

            // WE GET THE PLAYER.
            BattleUser currentUser = GetUser(socketUser);
            if (currentUser == null)
                return;

            // MAKE SURE PLAYER TURN.
            if (!_turnHandler.IsUnitTurn(currentUser))
                return;

            currentUser.UseSkill(useData);
        }
        private void PlayerBasicAttack(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            // WE GET THE PLAYER.
            BattleUser currentUser = GetUser(socketUser);
            if (currentUser == null)
                return;

            // MAKE SURE PLAYER TURN.
            if (!_turnHandler.IsUnitTurn(currentUser))
                return;

            // WE GET A RANDOM ENEMY.
            BattleAttackUseDTO attackUseData = requestData.GetRequestData<BattleAttackUseDTO>();
            BattleUnit targetEnemy = GetUnit(attackUseData.TargetUniqueId);
            if (targetEnemy == null)
                return;

            // ATTACK TO PLAYER.
            int damage = currentUser.GetDamage(targetEnemy);
            currentUser.AttackTo(targetEnemy, damage);

            // WE WILL SEND THE DAMAGE DATA.
            BattleAttackDTO attackData = new BattleAttackDTO();
            attackData.AddAttack(
                new BattleAttackItemDTO(currentUser.UniqueId,
                    targetEnemy.UniqueId,
                    damage
                )
            );
            SendToAllUsers(BattleActions.UnitBasicAttack, attackData);

            EndTurn();
        }
        private void BattleTillAnyPlayerTurn()
        {
            // WE LOOP TILL PLAYER TURN.
            BattleUnit attacker = _turnHandler.GetCurrentTurnUnit();
            if (attacker is BattleUser)
                return;

            // WE GET A RANDOM ENEMY.
            BattleUnit defender = _allUnits.Find(x => x.TeamIndex != attacker.TeamIndex && !x.IsDeath);
            if (defender == null)
                return;

            // ATTACK TO PLAYER.
            int damage = attacker.GetDamage(defender);
            attacker.AttackTo(defender, damage);

            // WE WILL SEND THE DAMAGE DATA.
            BattleAttackDTO attackData = new BattleAttackDTO();
            attackData.AddAttack(
                new BattleAttackItemDTO(attacker.UniqueId,
                    defender.UniqueId,
                    damage
                )
            );
            SendToAllUsers(BattleActions.UnitBasicAttack, attackData);

            // FINALIZE UNIT TURN AND SKIP TO NEXT UNIT.
            EndTurn();
        }
        private void SendAllReqDataToClient(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleUser? user = _users.FirstOrDefault(y => y.SocketUser == socketUser);

            BattleLoadAllDTO loadData = new BattleLoadAllDTO()
            {
                Difficulity = _difficulity,
                Waves = _waves.Select(y => new BattleWaveDTO
                {
                    Units = y.Units.Select(z => new BattleNpcUnitDTO
                    {
                        AttackSpeed = z.Stats.AttackSpeed,
                        Health = z.Health,
                        UniqueId = z.UniqueId,
                        Damage = z.Stats.Damage,
                        Position = z.Position,
                        MaxHealth = z.Stats.MaxHealth,
                        IsDead = z.IsDeath,
                        UnitId = z.UnitId,
                        TeamIndex = z.TeamIndex
                    }).ToArray()
                }).ToArray(),
                Players = _users.Select(z => new BattlePlayerDTO
                {
                    UniqueId = z.UniqueId,
                    AttackSpeed = z.Stats.AttackSpeed,
                    Health = z.Health,
                    Damage = z.Stats.Damage,
                    Position = z.Position,
                    IsRealPlayer = z.SocketUser == socketUser,
                    PlayerName = z.PlayerName,
                    MaxHealth = z.Stats.MaxHealth,
                    IsDead = z.IsDeath,
                    TeamIndex = z.TeamIndex,
                    Skills = z.Skills.Select(v => new BattleSkillDTO
                    {
                        LeftTurnToUse = v.LeftTurnToUse,
                        UniqueID = v.UniqueId,
                        Skill = v.Skill,
                        TurnCooldown = v.TurnCooldown,
                    }).ToArray()
                }).ToArray()
            };

            SendToUser(user, BattleActions.LoadAll, loadData);
        }

        public void SendToAllUsers(BattleActions battleAction, object data)
        {
            foreach (BattleUser user in _users)
            {
                SendToUser(user, battleAction, data);
            }
        }
        public void SendToUser(BattleUser user, BattleActions battleAction, object data)
        {
            if (!user.IsConnected)
                return;

            SocketResponse dataToSend = BattleActionResponseDTO.GetSuccess(
                user.DataId,
                battleAction,
                data
            );

            user.SocketUser.AddToUnExpectedAfterSendIt(dataToSend);
        }

        public BattleUser GetUser(SocketUser socketUser)
        {
            return _users.FirstOrDefault(y => y.SocketUser == socketUser);
        }
        public BattleUnit GetUnit(int targetUnitID)
        {
            BattleUser? user = _users.FirstOrDefault(y => y.UniqueId == targetUnitID);
            if (user != null)
                return user;

            BattleNpcUnit unit = _currentWave.Units.FirstOrDefault(y => y.UniqueId == targetUnitID);
            if (unit != null)
                return unit;

            return null;
        }
        public BattleUnit GetAliveEnemyUnit(BattleUnit owner)
        {
            return _currentWave.Units.OrderBy(y => Guid.NewGuid())
                .FirstOrDefault(y => !y.IsDeath && y.TeamIndex != owner.TeamIndex);
        }

        public void EndTurn()
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

                    Task.Run(() =>
                    {
                        CampaignController.CompleteCampaign(user.SocketUser,
                        userId: user.SocketUser.User.Id,
                        stageIndex: 1,
                        levelIndex: 1,
                        levelData: _difficulityData);
                    });
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
    }
}