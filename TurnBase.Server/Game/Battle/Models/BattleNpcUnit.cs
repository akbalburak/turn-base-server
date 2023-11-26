using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleNpcUnit : BattleUnit
    {
        public int UnitId { get; }
        public BattleNpcUnit(IMapDataEnemy enemyData, IAStarNode spawnNode)
        {
            UnitId = enemyData.Enemy;

            base.ChangeNode(spawnNode);
            base.LoadStats(new BattleUnitStats()
            {
                MaxHealth = enemyData.Health,
                AttackSpeed = enemyData.TurnSpeed,
                Damage = enemyData.Damage
            });
        }
    }
}
