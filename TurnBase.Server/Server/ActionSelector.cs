﻿using TurnBase.Server.Game.Controllers;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server
{
    public static class ActionSelector
    {
        public static SocketResponse ExecuteAction(ISocketUser socketUser, ISocketRequest request)
        {
            using (ISocketMethodParameter smp = new SocketMethodParameter(socketUser, request))
            {
                SocketResponse response = null;

                switch (request.Method)
                {
                    case Enums.ActionTypes.Login:
                        response = UserController.Login(smp);
                        break;
                    case Enums.ActionTypes.UserLevels:
                        response = UserLevelController.GetUserLevels(smp);
                        break;
                    case Enums.ActionTypes.StartBattle:
                        response = BattleController.StartABattle(smp);
                        break;
                    case Enums.ActionTypes.ExecuteBattleAction:
                        response = BattleController.ExecuteActionInBattle(smp);
                        break;
                    case Enums.ActionTypes.Ping:
                        response = PingController.Ping(smp);
                        break;
                    case Enums.ActionTypes.LoadItems:
                        response = ItemController.GetItems(smp);
                        break;
                    case Enums.ActionTypes.EquipItem:
                        response = InventoryController.EquipItem(smp);
                        break;
                    case Enums.ActionTypes.UnequipItem:
                        response = InventoryController.UnequipItem(smp);
                        break;
                    case Enums.ActionTypes.Parameters:
                        response = ParameterController.GetParameters(smp);
                        break;
                    case Enums.ActionTypes.UseItem:
                        response = InventoryController.UseAnItem(smp);
                        break;
                    case Enums.ActionTypes.LoadItemSkills:
                        response = ItemSkillController.GetItemSkills(smp);
                        break;
                    case Enums.ActionTypes.SwitchSkillSlot:
                        response = ItemSkillController.SwitchSkillSlot(smp);
                        break;
                    default:
                        Console.WriteLine("Methot Bulunamadı");
                        response = null;
                        break;
                }

                if (response != null && response.IsSuccess)
                {
                    smp.ExecuteOnSuccess();
                }

                return response;
            }
        }
    }
}
