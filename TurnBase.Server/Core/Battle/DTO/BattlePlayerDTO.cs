﻿using Newtonsoft.Json;

namespace TurnBase.Server.Core.Battle.DTO
{
    public class BattlePlayerDTO : BattleUnitDTO
    {
        [JsonProperty("Y")] public string PlayerName { get; set; }
        [JsonProperty("Z")] public bool IsRealPlayer { get; set; }
    }
}