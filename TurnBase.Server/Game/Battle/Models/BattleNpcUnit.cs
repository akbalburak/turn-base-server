using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.Skills;
using TurnBase.Server.Game.Services;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleNpcUnit : BattleUnit
    {
        public int UnitId { get; }

        public BattleNpcUnit(IMapDataEnemy enemyData)
        {
            UnitId = enemyData.Enemy;

            base.LoadStats(new BattleUnitStats()
            {
                MaxHealth = enemyData.Health,
                AttackSpeed = enemyData.TurnSpeed,
                Damage = enemyData.Damage
            });
        }

        public override void LoadSkills()
        {
            base.LoadSkills();

            // UNIQUE ID COUNTER FOR SKILLS.
            int uniqueSkillId = 0;

            // WE CREATE BASIC ATTACK SKILL.
            IItemSkill attackSkill = ItemSkillCreator.CreateSkill(
                uniqueId: ++uniqueSkillId,
                skill: ItemSkillService.GetItemSkill(Enums.ItemSkills.OneHandedBasicAttackSkill),
                battle: UnitData.BattleItem,
                owner: this,
                itemQuality: 1
            );

            this.AddSkill(attackSkill);

        }
    }
}
