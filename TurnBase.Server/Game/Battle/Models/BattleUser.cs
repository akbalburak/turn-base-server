using TurnBase.Server.Game.Battle.Interfaces;
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

        public BattleUser(ISocketUser socketUser,
            InventoryDTO inventory,
            int position,
            bool isFirstCompletion) : base(position)
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

            // WE LOOP ALL THE EQUIPMENTS.
            IUserItemDTO[] userItems = Inventory.GetEquippedItems();
            foreach (IUserItemDTO userItem in userItems)
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
                        ++uniqueSkillId,
                        skill,
                        Battle,
                        this,
                        userItem,
                        itemData
                    );

                    // IF SOME HOW SKILL UNDEFINED JUST SKIP.
                    if (battleSkill == null)
                        continue;

                    // OTHERWISE WE ADD NEW SKILL.
                    this.AddSkill(battleSkill);
                }
            }
        }
    }
}
