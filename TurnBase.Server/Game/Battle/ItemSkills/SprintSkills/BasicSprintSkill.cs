using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.SprintSkills
{
    public class BasicSprintSkill : BaseItemStackableSkill
    {
        public BasicSprintSkill(int uniqueId,
                                IItemSkillDTO skill,
                                IBattleItem battle,
                                IBattleUnit owner,
                                float itemQuality)
            : base(uniqueId, skill, battle, owner, itemQuality)
        {
        }

        protected override BattleSkillUsageDTO OnSkillUsing(BattleSkillUseDTO useData)
        {
            // WE MAKE SURE MOVE INDEX IS VALID.
            if (useData.TargetNodeIndex < 0 || useData.TargetNodeIndex >= Battle.NodeSize)
                return null;

            // WE GET THE POINTS.
            IAStarNode fromPoint = Owner.CurrentNode;
            IAStarNode targetPoint = Battle.GetNodeByIndex(useData.TargetNodeIndex);

            // WE LOOK FOR THE PATH.
            IAStarNode[] path = Battle.GetPath(fromPoint, targetPoint);

            // IF IN COMBAT, WE WILL MAKE SURE ENEMY CAN GO AS MUCH AS STACK SIZE.
            if (Battle.IsInCombat)
            {
                int movementCost = path.Length - 1;
                if (movementCost > base.CurrentStackSize)
                {
                    movementCost = base.CurrentStackSize;
                    Array.Resize(ref path, movementCost + 1);
                }
            }

            // IF NO PATH TO MOVE.
            if (path.Length == 0)
                return null;

            // IF ALREADY IN THE SAME LOCATION.
            IAStarNode lastValidPath = path.Last();
            int lastValidPathIndex = Battle.GetNodeIndex(lastValidPath);
            if (Owner.CurrentNode == lastValidPath)
                return null;

            // SKILL USAGE DATA FOR THE PLAYER.
            BattleSkillUsageDTO usageData = base.OnSkillUsing(useData);

            // ONLY IN COMBAT MOVEMENT COST IS SPENT.
            if (Battle.IsInCombat)
            {
                // CURRENT NODE SHOULD BE REMOVED FROM THE MOVEMENT COST.
                int movementCost = path.Length - 1;

                // IF SKILL STACK NOT ENOUGH JUST RETURN.
                if (!base.IsStackEnough(movementCost))
                    return null;

                base.UseStack(movementCost);
                usageData.AddAttribute(Enums.ItemSkillUsageAttributes.StackUsageCost, movementCost);
            }

            usageData.AddAttribute(Enums.ItemSkillUsageAttributes.Node, lastValidPathIndex);

            Owner.ChangeNode(lastValidPath);

            // WE TELL ALL THE UNITS IN RANGE TO AGGRO.
            foreach (IAStarNode pathNode in path)
                pathNode.TriggerAggro(Owner);

            return usageData;
        }

        public override int? GetNodeIndexForAI()
        {
            IBattleUnit enemy = Battle.GetAliveEnemyUnit(Owner);
            return Battle.GetNodeIndex(enemy.CurrentNode);
        }
    }
}
