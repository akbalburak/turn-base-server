using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.DTO.Interfaces
{
    public interface IItemSkillDataDTO
    {
        public ItemSkillData DataId { get; }
        public double GetValue(float quality);
    }
}
