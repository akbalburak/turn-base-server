using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.ItemSkills.Enums;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Item
{
    public interface IItemSkill
    {
        int UniqueId { get; }

        IItemSkillDTO SkillData { get; }
        IBattleUnit Owner { get; }

        int CurrentCooldown { get; }
        int InitialCooldown { get; }
        int UsageManaCost { get; }
        bool FinalizeTurnInUse { get; }
        float SkillQuality { get; }

        bool IsSkillReadyToUse();
        void UseSkill(BattleSkillUseDTO useData);

        BattleSkillDTO GetSkillDataDTO();
        int? GetNodeIndexForAI();
    }
}
