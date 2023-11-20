using Newtonsoft.Json;

namespace TurnBase.Server.Game.DTO
{
    public class LoginDTO
    {
        public class LoginRequestDTO
        {
            [JsonProperty("A")] public string Token { get; set; }
        }

        public class LoginResponseDTO
        {
            [JsonProperty("A")] public UserDTO User { get; set; }
        }
    }
}
