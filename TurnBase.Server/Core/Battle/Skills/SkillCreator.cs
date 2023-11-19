using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Skills
{
    public static class SkillCreator
    {
        public static BaseBattleSkill CreateSkill(int uniqueId,
            BattleSkills skill,
            IBattleItem battle,
            IBattleUnit unit
        )
        {
            return skill switch
            {
                BattleSkills.DoubleSlash => new BattleDoubleSlashSkill(uniqueId, battle, unit),
                _ => null,
            };
        }
    }
}
