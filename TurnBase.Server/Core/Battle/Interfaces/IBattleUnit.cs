using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Interfaces
{
    public interface IBattleUnit
    {
        Action OnUnitTurnStart { get; set; }

        int UniqueId { get; }

        int Position { get; }
        int TeamIndex { get; }

        int Health { get; }
        bool IsDeath { get; }
        
        BattleUnitStats Stats { get; }
        List<BaseBattleSkill> Skills { get; }

        int GetBaseDamage(IBattleUnit defender);

        void AttackToUnit(IBattleUnit defender, int damage);
        void ReduceHealth(int damage);

        void CallUnitTurnStart();

        void SetId(int id);
        void SetTeam(int teamIndex);
        void SetBattle(IBattleItem battleItem);

        void LoadSkills();
        void UseSkill(BattleSkillUseDTO useData);
    }
}
