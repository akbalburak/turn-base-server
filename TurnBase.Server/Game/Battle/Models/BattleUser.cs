using TurnBase.Server.Game.Battle.Core.Skills;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
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

                int index = -1;
                int[] skillSlots = itemData.Skills.Select(y => y.SlotIndex).Distinct().ToArray();
                foreach (int skillSlot in skillSlots)
                {
                    index++;

                    // WE MAKE SURE SLOT IS VALUD.
                    if (!userItem.TryGetSlotValue(index, out int selectedSlot))
                        continue;

                    // WE CHECK FOR THE SKILL DATA.
                    IItemSkillMappingDTO skill = itemData.GetItemSkill(skillSlot, selectedSlot);
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
