using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Services
{
    public static class SkillService
    {
        private static SkillDTO[] _skills = new SkillDTO[0];
        public static SkillDTO[] Skills => _skills;

        public static void Initialize()
        {
            using IUnitOfWork uow = new UnitOfWork();

            _skills = uow.GetRepository<TblSkill>()
                .Select(y => new SkillDTO
                {
                    Id = (BattleSkills)y.Id,
                    FinalizeTurnInUse = y.FinalizeTurnInUse,
                    TurnCooldown = y.TurnCooldown,
                }).ToArray();
        }

        public static SkillDTO GetSkill(BattleSkills skill)
        {
            return _skills.FirstOrDefault(x => x.Id == skill);
        }
    }
}
