using TurnBase.Server.Game.Battle.Map.Interfaces;

namespace TurnBase.Server.Game.Battle.Map
{
    public class MapDataFirstTimeRewardJson : IMapDataFirstTimeRewardJson
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public float Quality { get; set; }
    }
}
