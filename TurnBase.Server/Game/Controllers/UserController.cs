using TurnBase.DBLayer.Models;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Controllers
{
    public static class UserController
    {
        public static SocketResponse Login(ISocketMethodParameter smp)
        {
            LoginDTO.LoginRequestDTO requestData = smp.GetRequestData<LoginDTO.LoginRequestDTO>();
            Guid userAccessToken = Guid.Parse(requestData.Token);

            TblUser user = smp.UOW.GetRepository<TblUser>().Find(y => y.Token == userAccessToken);
            if (user == null)
                return SocketResponse.GetError("USER NOT FOUND!");

            smp.SocketUser.User.AssignUser(user);

            smp.UOW.SaveChanges();

            return SocketResponse.GetSuccess(new LoginDTO.LoginResponseDTO
            {
                User = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    UserLevel = user.UserLevel,
                    Experience = user.Experience,
                    Gold = user.Gold,
                    Inventory = user.Inventory,
                    Campaign = user.Campaign
                }
            });
        }
    }
}
