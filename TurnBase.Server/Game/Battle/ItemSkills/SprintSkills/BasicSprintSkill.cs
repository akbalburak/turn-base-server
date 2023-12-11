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

        public override bool IsSkillReadyToUse()
        {
            return !Battle.IsInCombat || base.IsSkillReadyToUse();
        }

        public override void OnSkillUse(BattleSkillUseDTO useData)
        {
            // WE MAKE SURE MOVE INDEX IS VALID.
            if (useData.TargetNodeIndex < 0 || useData.TargetNodeIndex >= Battle.NodeSize)
                return;

            // WE GET THE POINTS.
            IAStarNode fromPoint = Owner.CurrentNode;
            IAStarNode targetPoint = Battle.GetNodeByIndex(useData.TargetNodeIndex);

            // IF ALREADY IN THE SAME LOCATION.
            if (Owner.CurrentNode == targetPoint)
                return;

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

            if (path.Length == 0)
                return;

            IAStarNode lastValidPath = path.Last();
            int lastValidPathIndex = Battle.GetNodeIndex(lastValidPath);

            // SKILL USAGE DATA.
            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this);
            usageData.AddTargetNode(lastValidPathIndex);

            // ONLY IN COMBAT MOVEMENT COST IS SPENT.
            if (Battle.IsInCombat)
            {
                // CURRENT NODE SHOULD BE REMOVED FROM THE MOVEMENT COST.
                int movementCost = path.Length - 1;

                // IF SKILL STACK NOT ENOUGH JUST RETURN.
                if (!base.IsStackEnough(movementCost))
                    return;

                base.UseStack(movementCost);
                usageData.AddStackUsageCost(movementCost);
            }

            Owner.ChangeNode(lastValidPath);

            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);

            // WE TELL ALL THE UNITS IN RANGE TO AGGRO.
            foreach (IAStarNode pathNode in path)
                pathNode.TriggerAggro(Owner);
        }

        public override int? GetNodeIndexForAI()
        {
            IBattleUnit enemy = Battle.GetAliveEnemyUnit(Owner);
            return Battle.GetNodeIndex(enemy.CurrentNode);
        }
    }
}
