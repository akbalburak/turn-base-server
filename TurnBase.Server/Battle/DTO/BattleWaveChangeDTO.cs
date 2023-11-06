using Newtonsoft.Json;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleWaveChangeDTO
    {
        [JsonProperty("A")] public int WaveIndex { get; set; }
        public BattleWaveChangeDTO(int waveIndex)
        {
            this.WaveIndex = waveIndex;
        }
    }
}
