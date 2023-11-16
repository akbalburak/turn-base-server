using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Extends.Json;

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
