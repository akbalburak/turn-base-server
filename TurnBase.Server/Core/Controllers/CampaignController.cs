using TurnBase.DBLayer.Models;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Controllers
{
    public static class CampaignController
    {
        public static CampaignDTO GetCampaign(this TblUser user)
        {
            if (string.IsNullOrEmpty(user.Campaign))
                return new CampaignDTO();
            return user.Campaign.ToObject<CampaignDTO>();
        }
        public static void UpdateCampaign(this TblUser user, CampaignDTO campaign)
        {
            user.Campaign = campaign.ToJson();
        }
    }
}
