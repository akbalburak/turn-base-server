using TurnBase.Server.Core.Battle.Core;
using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Skills
{
    public static class SkillCreator
    {
        public static BaseBattleSkill CreateSkill(int id,
            BattleSkills skill,
            IBattleItem battle,
            BattleUnit unit
        )
        {
            return skill switch
            {
                BattleSkills.DoubleSlash => new BattleDoubleSlashSkill(id, battle, unit),
                _ => null,
            };
        }
    }
}
