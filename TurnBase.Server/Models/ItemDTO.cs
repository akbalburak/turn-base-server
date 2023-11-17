using Newtonsoft.Json;
using TurnBase.Server.Enums;

namespace TurnBase.Server.Models
{
    public class ItemDTO
    {
        [JsonProperty("A")] public int Id { get; set; }
        [JsonProperty("B")] public ItemTypes TypeId { get; set; }
        [JsonProperty("C")] public ItemPropertyDTO[] Properties { get; set; }
        [JsonProperty("D")] public ItemContentDTO[] Contents { get; set; }

        public ItemActions Action
        {
            get
            {
                switch (TypeId)
                {
                    case ItemTypes.Helmet:
                    case ItemTypes.Armor:
                    case ItemTypes.Shoes:
                    case ItemTypes.Bag:
                    case ItemTypes.MainHand:
                    case ItemTypes.OffHand:
                    case ItemTypes.Potion:
                    case ItemTypes.Food:
                        return ItemActions.Equipable;
                    case ItemTypes.MoneyBag:
                        return ItemActions.Usable;
                    default:
                        return ItemActions.Undefined;
                }
            }
        }
        public bool CanStack
        {
            get
            {
                switch (TypeId)
                {
                    case ItemTypes.Helmet:
                    case ItemTypes.Armor:
                    case ItemTypes.Shoes:
                    case ItemTypes.Bag:
                    case ItemTypes.MainHand:
                    case ItemTypes.OffHand:
                    default:
                        return false;
                    case ItemTypes.MoneyBag:
                    case ItemTypes.Potion:
                    case ItemTypes.Food:
                        return true;
                }
            }
        }

        public ItemDTO()
        {
            Properties = Array.Empty<ItemPropertyDTO>();
            Contents = Array.Empty<ItemContentDTO>();
        }
    }

    public class ItemPropertyDTO
    {
        [JsonProperty("A")] public ItemProperties PropertyId { get; set; }
        [JsonProperty("B")] public double MinValue { get; set; }
        [JsonProperty("C")] public double MaxValue { get; set; }

        public double GetValue(float quality)
        {
            double value = MinValue + (MaxValue - MinValue) * quality;
            return Math.Round(value, 2);
        }
    }

    public class ItemContentDTO
    {
        [JsonProperty("A")] public int ItemId { get; set; }
        [JsonProperty("B")] public ItemContents ContentId { get; set; }
        [JsonProperty("C")] public int? IndexId { get; set; }
        [JsonProperty("D")] public double Value { get; set; }

    }
}
