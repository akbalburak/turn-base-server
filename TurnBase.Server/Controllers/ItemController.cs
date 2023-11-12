using TurnBase.DBLayer.Models;
using TurnBase.Server.ServerModels;
using TurnBase.Server.Services.Item;

namespace TurnBase.Server.Controllers
{
    public static class ItemController
    {
        public static SocketResponse GetItems(SocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ItemService.Items);
        }
    }
}
