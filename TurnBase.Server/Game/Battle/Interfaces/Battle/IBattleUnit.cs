using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.Models;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IBattleUnit
    {
        Action<IBattleUnit> OnUnitTurnStart { get; set; }
        Action<IBattleUnit> OnUnitDie { get; set; }

        int UniqueId { get; }

        int Position { get; }
        int TeamIndex { get; }

        float PosX { get; }
        float PosZ { get; }

        int Health { get; }
        int Mana { get; }
        bool IsDeath { get; }

        BattleUnitStats Stats { get; }
        List<IItemSkill> Skills { get; }
        List<IItemSkillEffect> Effects { get; }

        int GetBaseDamage(IBattleUnit defender);

        void AttackToUnit(IBattleUnit defender, int damage);
        void ReduceHealth(int damage);

        void CallUnitTurnStart();

        void SetId(int id);
        void SetTeam(int teamIndex);
        void SetBattle(IBattleItem battleItem);

        void LoadSkills();
        void UseSkill(BattleSkillUseDTO useData);
        void AddEffect(IItemSkillEffect effect);
        bool IsManaEnough(int usageManaCost);
        void ReduceMana(int usageManaCost);


        void SetPosition(float x, float z);
    }
}
