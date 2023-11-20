using TurnBase.Server.Enums;
using TurnBase.Server.Models;

namespace TurnBase.Server.Interfaces
{
    public interface IItemDTO
    {
        int Id { get; }
        
        ItemTypes TypeId { get; }
        ItemActions Action { get; }

        bool CanStack { get; }

        ItemPropertyDTO[] Properties { get; }
        ItemSkillDTO[] Skills { get; }
        ItemContentDTO[] Contents { get; }

        IItemSkillDTO GetItemSkill(int skillSlot, int selectedSlot);
    }
}
