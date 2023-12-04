using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattlePlayerDTO : BattleUnitDTO
    {
        [JsonProperty("Y")] public string PlayerName { get; set; }
        [JsonProperty("Z")] public bool IsRealPlayer { get; set; }

        public BattlePlayerDTO(IBattleItem battleItem,
                               IBattleUser battleUser,
                               bool isRealPlayer)
            : base(battleItem, battleUser)
        {
            IsRealPlayer = isRealPlayer;
            PlayerName = battleUser.PlayerName;
        }
    }
}
