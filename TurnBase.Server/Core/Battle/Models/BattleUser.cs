using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Skills;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Core.Battle.Models
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
            // WE IMPLEMENT SKILLS.
            int skillId = 0;

            // WE LOOP ALL THE EQUIPMENTS.
            List<UserItemDTO> equippedItems = Inventory.Items.FindAll(y => y.Equipped);
            foreach (UserItemDTO equippedItem in equippedItems)
            {
                // SOMEHOW IF THE ITEM DOES NOT EXISTS.
                ItemDTO itemData = ItemService.GetItem(equippedItem.ItemID);
                if (itemData == null)
                    continue;

                // WE LOOP ALL THE SLOTS.
                foreach (int slot in equippedItem.SkillSlots)
                {
                    // IF SLOT SKILL NOT SELECTED JUST SKIP IT.
                    if (slot <= 0)
                        continue;

                    // WE CHECK FOR THE SKILL DATA.
                    ItemSkillDTO skill = itemData.Skills.FirstOrDefault(y => y.SlotIndex == slot);
                    if (skill == null)
                        continue;

                    // WE CREATE SKILL.
                    BaseBattleSkill battleSkill = SkillCreator.CreateSkill(
                        ++skillId,
                        (BattleSkills)skill.SkillId,
                        Battle,
                        this
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
