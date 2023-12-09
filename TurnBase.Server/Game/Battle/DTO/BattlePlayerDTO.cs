using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattlePlayerDTO : BattleUnitDTO
    {
        [JsonProperty("V")] public IInventoryItemDTO[] Equipments { get; set; }
        [JsonProperty("Y")] public string PlayerName { get; set; }
        [JsonProperty("Z")] public bool IsRealPlayer { get; set; }

        public BattlePlayerDTO(IBattleItem battleItem,
                               IBattleUser battleUser,
                               bool isRealPlayer)
            : base(battleItem, battleUser)
        {
            IsRealPlayer = isRealPlayer;
            PlayerName = battleUser.PlayerName;
            Equipments = battleUser.Equipments;
        }
    }
}
