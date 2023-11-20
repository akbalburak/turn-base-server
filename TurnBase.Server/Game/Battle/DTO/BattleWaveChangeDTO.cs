using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleWaveChangeDTO
    {
        [JsonProperty("A")] public int WaveIndex { get; set; }
        public BattleWaveChangeDTO(int waveIndex)
        {
            WaveIndex = waveIndex;
        }
    }
}
