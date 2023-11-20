using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.DTO
{
    public class ItemSkillDTO : IItemSkillDTO
    {
        public ItemSkills ItemSkill { get; set; }
        public bool FinalizeTurnInUse { get; set; }
        public int TurnCooldown { get; set; }

        public ItemSkillDataDTO[] Data { get; set; }
        public ItemSkillDTO()
        {
            Data = Array.Empty<ItemSkillDataDTO>();
        }

        public double GetDataValue(ItemSkillData data, IUserItemDTO userItem)
        {
            var skillData = Data.FirstOrDefault(y => y.DataId == data);
            if (skillData == null)
                return 0.0;
            return Math.Round(skillData.GetValue(userItem),2);
        }

        public int GetDataValueAsInt(ItemSkillData data, IUserItemDTO userItem)
        {
            return (int)Math.Floor(GetDataValue(data, userItem));
        }
    }

    public class ItemSkillDataDTO : IItemSkillDataDTO
    {
        public ItemSkillData DataId { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public double GetValue(IUserItemDTO userItem)
        {
            return MinValue + (MaxValue - MinValue) * userItem.Quality;
        }
    }
}
