namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleDropItem
    {
        int DropItemId { get; }

        int ItemId { get; }
        int Level { get; }
        float Quality { get; }
        bool Claimed { get; }
        int Quantity { get; }

        void SetAsClaimed();
    }
}