﻿using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.DTO.Interfaces
{
    public interface IItemDTO
    {
        int Id { get; }

        ItemTypes TypeId { get; }
        ItemActions Action { get; }

        bool CanStack { get; }

        ItemPropertyMappingDTO[] Properties { get; }
        ItemSkillMappingDTO[] Skills { get; }
        ItemContentMappingDTO[] Contents { get; }

        IItemSkillMappingDTO GetItemActiveSkill(int skillRow, int skillCol);
    }
}
