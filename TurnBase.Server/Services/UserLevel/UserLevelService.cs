﻿using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.DTOLayer.Models;

namespace TurnBase.Server.Services.UserLevel
{
    public static class UserLevelService
    {
        public static List<UserLevelDTO> UserLevels => _userLevels;

        private static List<UserLevelDTO> _userLevels = new List<UserLevelDTO>();

        public static void Initialize()
        {
            using IUnitOfWork uow = new UnitOfWork();

            _userLevels = uow.GetRepository<TblUserLevel>()
                .Select(y => new UserLevelDTO()
                {
                    Level = y.Id,
                    Experience = y.Experience,
                })
                .ToList();
        }

    }
}
