using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.Skills;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Services;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleUser : BattleUnit, IBattleUser
    {
        public Action<IBattleUser> OnUserConnected { get; set; }
        public Action<IBattleUser> OnUserDisconnected { get; set; }

        public IBattleInventory LootInventory { get; }
        public IInventoryItemDTO[] Equipments { get; set; }
        public ISocketUser SocketUser { get; private set; }

        public string PlayerName { get; private set; }
        public bool IsFirstCompletion { get; private set; }
        public bool IsReady { get; private set; }

        public bool IsConnected { get; private set; }

        public int GetNewDataId => ++_lastDataId;
        public int GetLastDataId => _lastDataId;


        private int _lastDataId;

        public BattleUser(ISocketUser socketUser,
            IEquipmentItemDTO[] equipments,
            bool isFirstCompletion)
        {
            UpdateSocketUser(socketUser);
            LootInventory = new BattleInventory(this);
            Equipments = equipments;
            PlayerName = socketUser.User.UserName;
            IsFirstCompletion = isFirstCompletion;

            base.LoadStats(new BattleUnitStats(equipments));
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
            foreach (IEquipmentItemDTO userItem in Equipments)
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
                        itemQuality: userItem.Quality,
                        inventoryItem: userItem
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
            if (SocketUser != null)
                SocketUser.OnUserTimeout -= OnUserTimeout;

            SocketUser = socketUser;
            SocketUser.OnUserTimeout += OnUserTimeout;

            SetAsConnected();
        }

        public void SetAsDisconnected()
        {
            if (IsConnected == false)
                return;

            IsConnected = false;
            OnUserDisconnected?.Invoke(this);
        }

        public void SetAsConnected()
        {
            if (IsConnected)
                return;

            IsConnected = true;
            OnUserConnected?.Invoke(this);
        }

        private void OnUserTimeout()
        {
            SetAsDisconnected();
        }

        public void SetAsReady()
        {
            IsReady = true;
        }
    }
}
