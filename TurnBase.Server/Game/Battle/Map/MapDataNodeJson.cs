using TurnBase.Server.Game.Battle.Map.Interfaces;
using TurnBase.Server.Serializables;

namespace TurnBase.Server.Game.Battle.Map.Json
{
    public class MapDataNodeJson : IMapDataNodeJson
    {
        public SerializableVector3 Node { get; private set; }

        public MapDataNodeJson(SerializableVector3 node)
        {
            Node = node;
        }
    }
}
