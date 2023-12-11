using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.DTO
{
    public class ItemSkillDTO : IItemSkillDTO
    {
        [JsonProperty("A")] public ItemSkills ItemSkill { get; private set; }
        [JsonProperty("B")] public bool FinalizeTurnInUse { get; private set; }
        [JsonProperty("C")] public ItemSkillDataDTO[] Data { get; private set; }
        [JsonProperty("D")] public ItemSkillShapes Shape { get; private set; }
        [JsonProperty("E")] public ItemSkillTargets Target { get; private set; }
        [JsonProperty("F")] public bool IsCombatSkill { get; private set; }

        public ItemSkillDTO(DBLayer.Models.TblItemSkill itemData)
        {
            IsCombatSkill = itemData.IsCombatSkill;
            Target = (ItemSkillTargets)itemData.TargetId;
            ItemSkill = (ItemSkills)itemData.Id;
            FinalizeTurnInUse = itemData.FinalizeTurnInUse;
            Shape = (ItemSkillShapes)itemData.ShapeId;
            Data = itemData.TblItemSkillDataMappings.Select(z => new ItemSkillDataDTO(z)).ToArray();
        }

        public double GetDataValue(ItemSkillData data, float quality)
        {
            ItemSkillDataDTO? skillData = Data.FirstOrDefault(y => y.DataId == data);
            if (skillData == null)
                return 0.0;
            return Math.Round(skillData.GetValue(quality), 2);
        }

        public int GetDataValueAsInt(ItemSkillData data, float quality)
        {
            return (int)Math.Floor(GetDataValue(data, quality));
        }
    }

    public class ItemSkillDataDTO : IItemSkillDataDTO
    {
        [JsonProperty("A")] public ItemSkillData DataId { get; set; }
        [JsonProperty("B")] public double MinQualityValue { get; set; }
        [JsonProperty("C")] public double MaxQualityValue { get; set; }

        public ItemSkillDataDTO(DBLayer.Models.TblItemSkillDataMapping skillDataMap)
        {
            DataId = (ItemSkillData)skillDataMap.ItemSkillDataId;
            MinQualityValue = skillDataMap.MinQualityValue;
            MaxQualityValue = skillDataMap.MinQualityValue;
        }

        public double GetValue(float quality)
        {
            quality = Math.Clamp(quality, 0, 1);

            if (MaxQualityValue > MinQualityValue)
                return MinQualityValue + (MaxQualityValue - MinQualityValue) * quality;
            else if (MinQualityValue > MaxQualityValue)
                return MaxQualityValue + (MinQualityValue - MaxQualityValue) * quality;
            else
                return MinQualityValue;
        }
    }

    public class ItemSkillSwitchDTO
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
        [JsonProperty("B")] public int RowIndex { get; set; }
        [JsonProperty("C")] public int ColIndex { get; set; }
    }
}
