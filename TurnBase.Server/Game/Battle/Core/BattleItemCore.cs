using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Map.Interfaces;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.Battle.Pathfinding.Core;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

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

        public IBattleTurnHandler BattleTurnHandler => _turnHandler;
        public IBattleUser[] Users => _users.ToArray();

        public bool GameOver { get; private set; }
        public string ChatRoom { get; }

        private IMapDataJson _levelData;
        private IBattleTurnHandler _turnHandler;

        private IBattleUser[] _users;
        private List<IBattleUnit> _allUnits;
        private List<IBattleNpcUnit> _allNpcs;
        private List<IBattleDrop> _drops;

        private bool _gameStarted;
        private bool _disposed;
        private Random _randomizer;


        public BattleItem(IBattleUser[] users, IMapDataJson levelData)
        {
            ChatRoom = $"{Guid.NewGuid()}";

            _drops = new List<IBattleDrop>();
            _allNpcs = new List<IBattleNpcUnit>();
            _allUnits = new List<IBattleUnit>();

            _turnHandler = new BattleTurnHandler(this);
            _randomizer = new Random();

            _users = users;
            _levelData = levelData;

            _nodes = _levelData.IMapHexNodes
                .Select(y => new AStarNode(y.Node.X, y.Node.Z))
                .ToArray();

            foreach (IAStarNode node in _nodes)
                node.FindNeighbors(_nodes, _levelData.DistancePerHex);

            int unitIdCounter = 0;

            // WE LOAD ALL UNITS REQUIRED DATA.
            foreach (IMapDataEnemyJson unitData in _levelData.IEnemies)
            {
                IAStarNode spawnNode = _nodes[unitData.SpawnIndex];
                BattleNpcUnit unit = new BattleNpcUnit(unitData);
                unit.OnUnitDie += OnUnitDie;
                unit.SetUnitData(new BattleUnitData(
                    uniqueId: ++unitIdCounter,
                    battleItem: this,
                    teamIndex: 2,
                    groupIndex: unitData.Group,
                    initialNode: spawnNode,
                    aggroDistance: unitData.AggroDistance,
                    drops: unitData.IDrops
                ));

                _allNpcs.Add(unit);
                _allUnits.Add(unit);
            }

            int[] spawnPoints = _levelData.IPlayerSpawnPoints;

            // WE LOAD ALL USERS REQUIRED DATA.
            int index = 0;
            foreach (IBattleUser user in _users)
            {
                int initialIndex = spawnPoints[index];
                IAStarNode node = _nodes[initialIndex];

                user.SetUnitData(new BattleUnitData(
                    uniqueId: ++unitIdCounter,
                    battleItem: this,
                    teamIndex: 1,
                    groupIndex: 0,
                    initialNode: node,
                    aggroDistance: 0,
                    drops: Array.Empty<IMapDataEnemyDropJson>()
                ));

                user.OnUserDisconnected += OnUserDisconnected;
                user.OnUserConnected += OnUserConnected;

                _allUnits.Add(user);
                index++;
            }
        }

        public void CallGroupAggrieving(int groupIndex)
        {
            // WE GET AGGRO UNITS.
            List<IBattleNpcUnit> aggroUnits = _allNpcs.FindAll(x => x.UnitData.GroupIndex == groupIndex && !x.IsAggrieved);
            if (aggroUnits.Count == 0)
                return;

            // WE TELL ALL AGGRO.
            aggroUnits.ForEach(x => x.OnAggrieved());

            // WE ADD THEM INTO TURN LIST.
            _turnHandler.AddUnits(aggroUnits);
        }

    }
}
