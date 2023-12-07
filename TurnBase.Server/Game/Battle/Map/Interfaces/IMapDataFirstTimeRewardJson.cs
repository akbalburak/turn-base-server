namespace TurnBase.Server.Game.Battle.Map.Interfaces
{
    public interface IMapDataFirstTimeRewardJson
    {
        public int ItemId { get; }
        public int Quantity { get; }
        public int Level { get; }
        public float Quality { get; }
    }
}
