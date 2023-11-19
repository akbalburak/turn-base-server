using TurnBase.Server.Core.Battle.Enums;

namespace TurnBase.Server.Models
{
    public class SkillDTO
    {
        public BattleSkills Id { get; set; }
        public bool FinalizeTurnInUse { get; internal set; }
        public int TurnCooldown { get; internal set; }
    }
}
