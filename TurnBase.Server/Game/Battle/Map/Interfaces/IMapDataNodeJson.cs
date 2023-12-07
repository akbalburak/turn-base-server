using TurnBase.Server.Serializables;

namespace TurnBase.Server.Game.Battle.Map.Interfaces
{
    public interface IMapDataNodeJson
    {
        SerializableVector3 Node { get; }
    }
}
