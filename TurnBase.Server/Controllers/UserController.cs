﻿using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Controllers
{
    public static class UserController
    {
        public static SocketResponse Login(SocketMethodParameter smp)
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
