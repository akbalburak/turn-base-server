using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleDropItem : IStoreableItemDTO
    {
        int DropItemId { get; }
        bool Claimed { get; }
        
        void SetAsClaimed();
    }
}