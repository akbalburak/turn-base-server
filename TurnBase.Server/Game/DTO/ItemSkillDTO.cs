using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.DTO
{
    public class ItemSkillDTO : IItemSkillDTO
    {
        [JsonProperty("A")] public ItemSkills ItemSkill { get; set; }
        [JsonProperty("B")] public bool FinalizeTurnInUse { get; set; }
        [JsonProperty("C")] public int TurnCooldown { get; set; }
        [JsonProperty("D")] public ItemSkillDataDTO[] Data { get; set; }
        [JsonProperty("E")] public int UsageManaCost { get; set; }
        public ItemSkillDTO()
        {
            Data = Array.Empty<ItemSkillDataDTO>();
        }

        public double GetDataValue(ItemSkillData data, IUserItemDTO userItem)
        {
            var skillData = Data.FirstOrDefault(y => y.DataId == data);
            if (skillData == null)
                return 0.0;
            return Math.Round(skillData.GetValue(userItem), 2);
        }

        public int GetDataValueAsInt(ItemSkillData data, IUserItemDTO userItem)
        {
            return (int)Math.Floor(GetDataValue(data, userItem));
        }
    }

    public class ItemSkillDataDTO : IItemSkillDataDTO
    {
        [JsonProperty("A")] public ItemSkillData DataId { get; set; }
        [JsonProperty("B")] public double MinValue { get; set; }
        [JsonProperty("C")] public double MaxValue { get; set; }

        public double GetValue(IUserItemDTO userItem)
        {
            return MinValue + (MaxValue - MinValue) * userItem.Quality;
        }
    }

    public class ItemSkillSwitchDTO
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
        [JsonProperty("B")] public int RowIndex { get; set; }
        [JsonProperty("C")] public int ColIndex { get; set; }
    }
}
