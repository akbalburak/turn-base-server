using Newtonsoft.Json;
using TurnBase.Server.Enums;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Models
{
    public class UserGoldDTO : IChangeItem
    {
        [JsonProperty("A")] public int AddCoins { get; }

        public UserGoldDTO(int addCoins)
        {
            AddCoins = addCoins;
        }

        public SocketResponse GetResponse()
        {
            return SocketResponse.GetSuccess(ActionTypes.UserGoldChanged, this);
        }
    }
}
