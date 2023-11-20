using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Interfaces;

namespace TurnBase.Server.Models
{
    public class SkillDTO : ISkillDTO
    {
        public BattleSkills Id { get; set; }
        public bool FinalizeTurnInUse { get; internal set; }
        public int TurnCooldown { get; internal set; }
    }
}
