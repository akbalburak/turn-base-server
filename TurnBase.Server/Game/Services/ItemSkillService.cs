﻿using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Services
{
    public static class ItemSkillService
    {
        private static ItemSkillDTO[] _itemSkills = new ItemSkillDTO[0];
        public static ItemSkillDTO[] ItemSkills => _itemSkills;

        public static void Initialize()
        {
            using IUnitOfWork uow = new UnitOfWork();

            _itemSkills = uow.GetRepository<TblItemSkill>()
                .Include(itemSkill => itemSkill.TblItemSkillDataMappings)
                .Select(itemSkill => new ItemSkillDTO(itemSkill))
                .ToArray();
        }

        public static IItemSkillDTO GetItemSkill(ItemSkills skill)
        {
            return _itemSkills.FirstOrDefault(x => x.ItemSkill == skill);
        }
    }
}
