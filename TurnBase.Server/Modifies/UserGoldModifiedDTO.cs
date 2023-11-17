using Newtonsoft.Json;
using TurnBase.Server.Enums;
using TurnBase.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Modifies
{
    public class UserGoldModifiedDTO : IChangeItem
    {
        [JsonProperty("A")] public int AddCoins { get; }

        public UserGoldModifiedDTO(int addCoins)
        {
            AddCoins = addCoins;
        }

        public SocketResponse GetResponse()
        {
            return SocketResponse.GetSuccess(ActionTypes.UserGoldModified, this);
        }
    }
}
