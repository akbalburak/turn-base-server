using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Item
{
    public interface IItemConsumableSkill : IItemSkill
    {
        int LeftUseCount { get; }
        int UsageCount { get; }
        IInventoryItemDTO InventoryItem { get; }
    }
}
