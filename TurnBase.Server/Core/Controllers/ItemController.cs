using TurnBase.DBLayer.Models;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Controllers
{
    public static class ItemController
    {
        public static SocketResponse GetItems(SocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ItemService.Items);
        }
    }
}
