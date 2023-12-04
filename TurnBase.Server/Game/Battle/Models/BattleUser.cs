using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.Skills;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Game.Services;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleUser : BattleUnit, IBattleUser
    {
        public InventoryDTO Inventory { get; set; }
        public ISocketUser SocketUser { get; private set; }
        public string PlayerName { get; private set; }
        public bool IsFirstCompletion { get; private set; }

        public bool IsConnected => SocketUser != null;

        private int _lastDataId;
        public int GetNewDataId => ++_lastDataId;
        public int GetLastDataId => _lastDataId;

        public BattleUser(ISocketUser socketUser,
            InventoryDTO inventory,
            bool isFirstCompletion)
        {
            Inventory = inventory;
            SocketUser = socketUser;
            PlayerName = socketUser.User.UserName;
            IsFirstCompletion = isFirstCompletion;

            base.LoadStats(new BattleUnitStats(inventory));
        }

        public override void LoadSkills()
        {
            // UNIQUE ID COUNTER FOR SKILLS.
            int uniqueSkillId = 0;

            // WE CREATE MOVEMENT SKILL.
            IItemSkill sprintSkill = ItemSkillCreator.CreateSkill(
                uniqueId: ++uniqueSkillId,
                skill: ItemSkillService.GetItemSkill(Enums.ItemSkills.BasicSprint),
                battle: UnitData.BattleItem,
                owner: this,
                itemQuality: 1
            );

            this.AddSkill(sprintSkill);

            // WE CREATE BASIC ATTACK SKILL.
            IItemSkill attackSkill = ItemSkillCreator.CreateSkill(
                uniqueId: ++uniqueSkillId,
                skill: ItemSkillService.GetItemSkill(Enums.ItemSkills.OneHandedBasicAttackSkill),
                battle: UnitData.BattleItem,
                owner: this,
                itemQuality: 1
            );

            this.AddSkill(attackSkill);

            // WE LOOP ALL THE EQUIPMENTS.
            IInventoryItemDTO[] userItems = Inventory.GetEquippedItems();
            foreach (IInventoryItemDTO userItem in userItems)
            {
                // SOMEHOW IF THE ITEM DOES NOT EXISTS.
                IItemDTO itemData = ItemService.GetItem(userItem.ItemID);
                if (itemData == null)
                    continue;

                // WE GET ALL THE DISTINCT SKILL ROWS.
                int[] skillRows = itemData.Skills.Select(y => y.RowIndex).Distinct().ToArray();
                foreach (int row in skillRows)
                {
                    // WE MAKE SURE THERE IS A SKILL IN GIVEN ROW.
                    if (!userItem.TryGetSelectedSkillCol(row, out int selectedSkillCol))
                        continue;

                    // WE MAKE SURE SELECTED COL FROM SKILLS IS VALID.
                    IItemSkillMappingDTO skillMap = itemData.GetItemActiveSkill(row, selectedSkillCol);
                    if (skillMap == null)
                        continue;

                    // WE GET SELECT SKILL.
                    IItemSkillDTO skill = ItemSkillService.GetItemSkill(skillMap.ItemSkill);
                    if (skill == null)
                        continue;

                    // WE CREATE SKILL.
                    IItemSkill battleSkill = ItemSkillCreator.CreateSkill(
                        uniqueId: ++uniqueSkillId,
                        skill: skill,
                        battle: UnitData.BattleItem,
                        owner: this,
                        userItem.Quality
                    );

                    // IF SOME HOW SKILL UNDEFINED JUST SKIP.
                    if (battleSkill == null)
                        continue;

                    // OTHERWISE WE ADD NEW SKILL.
                    this.AddSkill(battleSkill);
                }
            }
        }

        public void UpdateSocketUser(ISocketUser socketUser)
        {
            SocketUser = socketUser;
        }
    }
}
