using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;
using TurnBase.Server.Game.Interfaces;

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
            if (path.Length == 0)
                return;

            // CURRENT NODE SHOULD BE REMOVED FROM THE MOVEMENT COST.
            int movementCost = path.Length - 1;

            // IF SKILL STACK NOT ENOUGH JUST RETURN.
            if (!base.IsStackEnough(movementCost)) 
                return;

            base.UseStack(movementCost);

            Owner.ChangeNode(targetPoint);

            // SKILL USAGE DATA.
            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this);
            usageData.AddTargetNode(useData.TargetNodeIndex);
            usageData.AddStackUsageCost(movementCost);

            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);
        }
    }
}
