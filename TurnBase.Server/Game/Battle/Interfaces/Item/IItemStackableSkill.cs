using TurnBase.Server.Game.Battle.DTO;

namespace TurnBase.Server.Game.Battle.Interfaces.Item
{
    public interface IItemStackableSkill : IItemSkill
    {
        int InitialStackSize { get; }
        int CurrentStackSize { get; }
    }
}
