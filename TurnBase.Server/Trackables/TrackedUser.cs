using TurnBase.DBLayer.Models;
using TurnBase.Server.Core.Controllers;
using TurnBase.Server.Interfaces;
using TurnBase.Server.Models;
using TurnBase.Server.Modifies;

namespace TurnBase.Server.Trackables
{
    public class TrackedUser
    {
        private IChangeManager _changeManager;
        private TblUser _user;

        public TrackedUser(TblUser user,IChangeManager changeManager)
        {
            _changeManager = changeManager;
            _user = user;
        }

        public InventoryDTO GetInventory()
        {
            return _user.GetInventory(_changeManager);
        }
        public void UpdateInventory(InventoryDTO inventory)
        {
            _user.UpdateInventory(inventory);
        }

        public CampaignDTO GetCampaign()
        {
            return _user.GetCampaign();
        }
        public void UpdateCampaign(CampaignDTO campaign)
        {
            _user.UpdateCampaign(campaign);
        }

        public void AddGolds(int coin)
        {
            _user.Gold += coin;
            _changeManager.AddChanges(new UserGoldModifiedDTO(coin));
        }

    }
}
