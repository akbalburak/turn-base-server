namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleDropItem
    {
        int ItemId { get; }
        int Level { get; }
        float Quality { get; }
        bool Claimed { get; }

        void SetAsClaimed();
    }
}