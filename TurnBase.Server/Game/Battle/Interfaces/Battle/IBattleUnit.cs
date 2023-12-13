using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IBattleUnit : IAStarUnit
    {
        Action<IBattleUnit> OnUnitTurnStart { get; set; }
        Action<IBattleUnit> OnUnitDie { get; set; }

        int Health { get; }
        void IncreaseHealth(int health);
        int Mana { get; }
        bool IsAggrieved { get; }

        BattleUnitStats Stats { get; }
        List<IItemSkill> Skills { get; }
        List<IItemSkillEffect> Effects { get; }

        bool IsDeath { get; }
        int GetBaseDamage(IBattleUnit defender);
        void AttackToUnit(IBattleUnit defender, int damage);
        void HitUnit(IBattleUnit attacker, int damage);
        IBattleUnit KilledBy { get; }

        void CallUnitTurnStart();

        void SetUnitData(IBattleUnitData unitData);

        IItemConsumableSkill[] GetConsumableSkills();
        void AddSkill(IItemSkill skill);
        void LoadSkills();
        void UseSkill(BattleSkillUseDTO useData);

        void AddEffect(IItemSkillEffect effect);

        bool IsManaEnough(int usageManaCost);
        void UseMana(int usageManaCost);
        bool IsAnEnemy(IBattleUnit owner);
        void UseAI();
    }
}
