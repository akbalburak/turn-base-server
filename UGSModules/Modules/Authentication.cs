using Microsoft.Extensions.Logging;
using ModuleDTOLayer;
using Newtonsoft.Json;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

namespace UGSModules.Modules
{
    public class Authentication
    {
        private readonly ILogger<Authentication> _logger;
        public Authentication(ILogger<Authentication> logger)
        {
            _logger = logger;
        }

        [CloudCodeFunction("Login")]
        public async Task<UserDTO> Login(IExecutionContext ctx, IGameApiClient gameApiClient)
        {
            var currencies = await gameApiClient.EconomyCurrencies
                .GetPlayerCurrenciesAsync(ctx,
                    ctx.AccessToken,
                    ctx.ProjectId,
                    ctx.PlayerId
                );

            var playerData = await gameApiClient.CloudSaveData.GetItemsAsync(
                ctx,
                ctx.AccessToken,
                ctx.ProjectId,
                ctx.PlayerId,
                keys: new List<string>()
                {
                    "USER_DATA",
                    "CAMPAIGN",
                    "INVENTORY"
                }
            );

            UserDTO user = new UserDTO();
            user.UserId = ctx.UserId;
            user.Coins = (int)currencies.Data.Results.Find(x => x.CurrencyId == "COINS").Balance;

            // MEANS
            if (!playerData.Data.Results.Exists(x => x.Key == "USER_DATA"))
            {

                user.UserData = new UserDataDTO()
                {
                    Experience = 0,
                    UserLevel = 1,
                    Username = Guid.NewGuid().ToString().Substring(0, 6)
                };

                await gameApiClient.CloudSaveData.SetItemAsync(
                    ctx,
                    ctx.AccessToken,
                    ctx.ProjectId,
                    ctx.PlayerId,
                    new Unity.Services.CloudSave.Model.SetItemBody("USER_DATA", user.UserData)
                );
            }
            else
            {
                object userData = playerData.Data.Results.Find(x => x.Key == "USER_DATA").Value;
                user.UserData = JsonConvert.DeserializeObject<UserDataDTO>(userData.ToString());
            }

            return user;
        }
    }
}
