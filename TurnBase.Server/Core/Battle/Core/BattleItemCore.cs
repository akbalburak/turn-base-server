﻿using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Enums;

namespace TurnBase.Server.Core.Battle.Core
{
    public partial class BattleItem : IBattleItem, IDisposable
    {
        public Action<IBattleItem> OnDisposed { get; set; }

        private BattleLevelData _levelData;
        private IBattleTurnHandler _turnHandler;

        private IBattleUser[] _users;
        private List<IBattleUnit> _allUnits;

        private BattleDifficulityData _difficulityData;
        private LevelDifficulities _difficulity;

        private BattleWave[] _waves;
        private BattleWave _currentWave;

        private bool _gameStarted;
        private bool _gameOver;
        private bool _disposed;

        public BattleItem(IBattleUser[] users, BattleLevelData levelData, LevelDifficulities difficulity)
        {
            _users = users;

            // WE LOAD LEVEL DATA AND DIFFICULITY DATA.
            _levelData = levelData;
            _difficulity = difficulity;
            _difficulityData = levelData.GetDifficulityData(difficulity);

            // WE ACTIVATE THE FIRST WAVE.
            _waves = _difficulityData.Waves.ToArray();
            _currentWave = _waves[0];

            int unitIdCounter = 0;

            // WE LOAD ALL UNITS REQUIRED DATA.
            foreach (BattleWave wave in _waves)
            {
                foreach (IBattleUnit unit in wave.Units)
                {
                    unit.SetBattle(this);
                    unit.SetId(++unitIdCounter);
                    unit.SetTeam(2);
                    unit.LoadSkills();
                }
            }

            // WE LOAD ALL USERS REQUIRED DATA.
            foreach (IBattleUser user in _users)
            {
                user.SetBattle(this);
                user.SetId(++unitIdCounter);
                user.SetTeam(1);
                user.LoadSkills();
            }

            // WE COMBINE ALL THE UNITS.
            _allUnits = new List<IBattleUnit>();
            _allUnits.AddRange(_currentWave.Units);
            _allUnits.AddRange(_users);

            // WE CREATE TURN HANDLER.
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
    }
}
