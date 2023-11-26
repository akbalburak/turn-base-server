using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.Battle.Pathfinding.Core;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem : IBattleItem, IDisposable
    {
        public Action<IBattleItem> OnDisposed { get; set; }
        public double GetRandomValue => _randomizer.NextDouble();

        private BattleLevelData _levelData;
        private IBattleTurnHandler _turnHandler;

        private IBattleUser[] _users;
        private List<IBattleUnit> _allUnits;
        private List<BattleNpcUnit> _allNpcs;

        private BattleDifficulityData _difficulityData;
        private LevelDifficulities _difficulity;

        private BattleWave[] _waves;
        private BattleWave _currentWave;

        private bool _gameStarted;
        private bool _gameOver;
        private bool _disposed;
        private Random _randomizer;

        private AStarNode[] _nodes;

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

            // WE ACTIVATE THE FIRST WAVE.
            _waves = _difficulityData.Waves.ToArray();
            _currentWave = _waves[0];

            _nodes = _currentWave.PathData
                .MapHexNodes
                .Select(y => new AStarNode(y.Node.X, y.Node.Z))
                .ToArray();

            foreach (IAStarNode node in _nodes)
            {
                node.FindNeighbors(_nodes, _currentWave.PathData.DistancePerHex);
            }

            int unitIdCounter = 0;

            // WE LOAD ALL UNITS REQUIRED DATA.
            foreach (IMapDataEnemy unitData in _currentWave.PathData.Enemies)
            {
                IAStarNode spawnNode = _nodes[unitData.SpawnIndex];
                BattleNpcUnit unit = new BattleNpcUnit(unitData, spawnNode);

                unit.SetBattle(this);
                unit.SetId(++unitIdCounter);
                unit.SetTeam(2);
                unit.LoadSkills();

                _allNpcs.Add(unit);
                _allUnits.Add(unit);
            }

            // WE LOAD ALL USERS REQUIRED DATA.
            foreach (IBattleUser user in _users)
            {
                user.SetBattle(this);
                user.SetId(++unitIdCounter);
                user.SetTeam(1);
                user.LoadSkills();

                int initialIndex = _currentWave.PathData.PlayerSpawnPoints[0];
                IAStarNode node = _nodes[initialIndex];
                user.ChangeNode(node);

                _allUnits.Add(user);
            }

            // WE CREATE TURN HANDLER.
            _turnHandler = new BattleTurnHandler(this);
            _turnHandler.AddUnits(_allUnits);
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
