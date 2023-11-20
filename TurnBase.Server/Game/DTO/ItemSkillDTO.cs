using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.DTO
{
    public class ItemSkillDTO : IItemSkillDTO
    {
        public BattleSkills Id { get; set; }
        public bool FinalizeTurnInUse { get; internal set; }
        public int TurnCooldown { get; internal set; }
    }
}
