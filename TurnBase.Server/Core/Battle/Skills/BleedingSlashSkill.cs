using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Skills
{
    public class BleedingSlashSkill : BaseSkill
    {
        public BleedingSlashSkill(int uniqueId, BattleSkills skill, IBattleItem battle, IBattleUnit unit) 
            : base(uniqueId, skill, battle, unit)
        {
        }

        public override void OnSkillUse(BattleSkillUseDTO useData)
        {
        }
    }
}
