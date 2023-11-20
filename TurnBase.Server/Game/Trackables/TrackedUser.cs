using TurnBase.DBLayer.Models;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Trackables
{
    public class TrackedUser
    {
        private IChangeHandler _changeHandler;
        private TblUser _user;

        public TrackedUser(TblUser user, IChangeHandler changeHandler)
        {
            _changeHandler = changeHandler;
            _user = user;
        }

        public InventoryDTO GetInventory()
        {
            InventoryDTO inventory;

            if (string.IsNullOrEmpty(_user.Inventory))
                inventory = new InventoryDTO();
            else
                inventory = _user.Inventory.ToObject<InventoryDTO>();

            inventory.SetChangeHandler(_changeHandler);

            return inventory;
        }
        public void UpdateInventory(InventoryDTO inventory)
        {
            _user.Inventory = inventory.ToJson();
        }

        public CampaignDTO GetCampaign()
        {
            CampaignDTO campaign;

            if (string.IsNullOrEmpty(_user.Campaign))
                campaign = new CampaignDTO();
            else
                campaign = _user.Campaign.ToObject<CampaignDTO>();

            campaign.SetChangeHandler(_changeHandler);

            return campaign;
        }
        public void UpdateCampaign(CampaignDTO campaign)
        {
            _user.Campaign = campaign.ToJson();
        }

        public void AddGolds(int coin)
        {
            _user.Gold += coin;
            _changeHandler.AddChanges(new UserGoldDTO(coin));
        }

    }
}
