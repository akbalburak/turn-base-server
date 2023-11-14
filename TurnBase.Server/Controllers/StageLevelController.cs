using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Extends.Json;

namespace TurnBase.Server.Controllers
{
    public static class StageLevelController
    {
        public static CampaignDTO GetCampaign(TblUser user)
        {
            if (string.IsNullOrEmpty(user.Campaign))
                return new CampaignDTO();
            return user.Campaign.ToObject<CampaignDTO>();
        }
        public static void UpdateCampaign(TblUser user, CampaignDTO campaign)
        {
            user.Campaign = campaign.ToJson();
        }
    }
}
