using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.DTO
{
    public class ItemDTO : IItemDTO
    {
        [JsonProperty("A")] public int Id { get; set; }
        [JsonProperty("B")] public ItemTypes TypeId { get; set; }
        [JsonProperty("C")] public ItemPropertyMappingDTO[] Properties { get; set; }
        [JsonProperty("D")] public ItemContentMappingDTO[] Contents { get; set; }
        [JsonProperty("E")] public ItemSkillMappingDTO[] Skills { get; set; }

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
            Properties = Array.Empty<ItemPropertyMappingDTO>();
            Contents = Array.Empty<ItemContentMappingDTO>();
        }

        public IItemSkillMappingDTO GetItemActiveSkill(int skillRow, int skillCol)
        {
            return Skills.Where(y => y.RowIndex == skillRow)
                        .Skip(skillCol)
                        .FirstOrDefault();
        }

    }

    public class ItemPropertyMappingDTO
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

    public class ItemContentMappingDTO
    {
        [JsonProperty("A")] public ItemContents ContentId { get; set; }
        [JsonProperty("B")] public int? IndexId { get; set; }
        [JsonProperty("C")] public double Value { get; set; }

    }

    public class ItemSkillMappingDTO : IItemSkillMappingDTO
    {
        [JsonProperty("A")] public ItemSkills ItemSkill { get; set; }
        [JsonProperty("B")] public int RowIndex { get; set; }
        [JsonProperty("C")] public int ColIndex { get; set; }
    }
}
