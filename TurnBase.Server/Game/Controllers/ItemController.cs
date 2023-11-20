using TurnBase.Server.Game.Services;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Controllers
{
    public static class ItemController
    {
        public static SocketResponse GetItems(ISocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ItemService.Items);
        }
    }
}
