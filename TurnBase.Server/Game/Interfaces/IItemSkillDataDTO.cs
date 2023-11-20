using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Interfaces
{
    public interface IItemSkillDataDTO
    {
        public ItemSkillData DataId { get; }
        public double GetValue(IUserItemDTO userItem);
    }
}
