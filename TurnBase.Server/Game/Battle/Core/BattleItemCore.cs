﻿using TurnBase.Server.Extends;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.Battle.Pathfinding.Core;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem : IBattleItem, IBattlePath, IDisposable
    {
        public Action<IBattleItem> OnDisposed { get; set; }

        public double GetRandomValue => _randomizer.NextDouble();

        public bool IsInCombat
        {
            get
            {
                if (_turnHandler == null)
                    return false;
                return _turnHandler.IsInCombat;
            }
        }

        private BattleLevelData _levelData;
        private IBattleTurnHandler _turnHandler;

        private IBattleUser[] _users;
        private List<IBattleUnit> _allUnits;
        private List<BattleNpcUnit> _allNpcs;

        private BattleDifficulityData _difficulityData;
        private LevelDifficulities _difficulity;

        private bool _gameStarted;
        private bool _gameOver;
        private bool _disposed;
        private Random _randomizer;

        public BattleItem(IBattleUser[] users, BattleLevelData levelData, LevelDifficulities difficulity)
        {
            _allNpcs = new List<BattleNpcUnit>();
            _allUnits = new List<IBattleUnit>();

            _randomizer = new Random();
            _users = users;

            // WE LOAD LEVEL DATA AND DIFFICULITY DATA.
            _levelData = levelData;
            _difficulity = difficulity;
            _difficulityData = levelData.GetDifficulityData(difficulity);

            _nodes = _difficulityData.MapData.MapHexNodes
                .Select(y => new AStarNode(y.Node.X, y.Node.Z))
                .ToArray();

            foreach (IAStarNode node in _nodes)
            {
                node.FindNeighbors(_nodes, _difficulityData.MapData.DistancePerHex);
            }

            int unitIdCounter = 0;

            // WE LOAD ALL UNITS REQUIRED DATA.
            foreach (IMapDataEnemy unitData in _difficulityData.MapData.Enemies)
            {
                IAStarNode spawnNode = _nodes[unitData.SpawnIndex];
                BattleNpcUnit unit = new BattleNpcUnit(unitData);

                unit.SetUnitData(new BattleUnitData(
                    uniqueId: ++unitIdCounter,
                    battleItem: this,
                    teamIndex: 2,
                    groupIndex: unitData.Group,
                    initialNode: spawnNode,
                    aggroDistance: unitData.AggroDistance
                ));

                _allNpcs.Add(unit);
                _allUnits.Add(unit);
            }

            // WE LOAD ALL USERS REQUIRED DATA.
            foreach (IBattleUser user in _users)
            {
                int initialIndex = _difficulityData.MapData.PlayerSpawnPoints[0];
                IAStarNode node = _nodes[initialIndex];

                user.SetUnitData(new BattleUnitData(
                    uniqueId: ++unitIdCounter,
                    battleItem: this,
                    teamIndex: 1,
                    groupIndex: 0,
                    initialNode: node,
                    aggroDistance: 0
                ));

                _allUnits.Add(user);
            }

            // WE CREATE TURN HANDLER.
            _turnHandler = new BattleTurnHandler(this);
            _turnHandler.AddUnits(_users);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _gameOver = true;

            OnDisposed?.Invoke(this);
        }

        public void CallGroupAggrieving(int groupIndex)
        {
            List<BattleNpcUnit> aggroUnits = _allNpcs.FindAll(x => x.UnitData.GroupIndex == groupIndex);
            aggroUnits.ForEach(x => x.OnAggrieved());

            _turnHandler.AddUnits(aggroUnits);

            BattleTillAnyPlayerTurn();
        }
    }
}
