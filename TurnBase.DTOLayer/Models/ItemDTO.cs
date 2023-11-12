using Newtonsoft.Json;
using TurnBase.DTOLayer.Enums;

namespace TurnBase.DTOLayer.Models
{
    public class ItemDTO
    {
        [JsonProperty("A")] public int Id { get; set; }
        [JsonProperty("B")] public ItemTypes TypeId { get; set; }
        [JsonProperty("C")] public ItemPropertyDTO[] Properties { get; set; }

        public ItemDTO()
        {
            Properties = Array.Empty<ItemPropertyDTO>();
        }
    }

    public class ItemPropertyDTO
    {
        [JsonProperty("A")] public ItemProperties PropertyId { get; set; }
        [JsonProperty("B")] public double MinValue { get; set; }
        [JsonProperty("C")] public double MaxValue { get; set; }

        public double GetValue(int quality)
        {
            double value = MinValue + (MaxValue - MinValue) * (quality / 100f);
            return Math.Round(value, 2);
        }
    }
}
