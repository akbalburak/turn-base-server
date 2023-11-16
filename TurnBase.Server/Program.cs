using Newtonsoft.Json;
using TurnBase.Server;
using TurnBase.Server.Services;

internal class Program
{
    public static Guid AppID = Guid.NewGuid();

    public const int TCP_PORT = 4200;

    private static void Main(string[] args)
    {

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        ParameterService.Initialize();
        ItemService.Initialize();
        BattleLevelService.Initialize();
        SocketUserServices.Initialize();
        UserLevelService.Initialize();

        TcpServer server = new TcpServer(TCP_PORT);

        Console.WriteLine("Server Created");

        Console.ReadLine();
    }

    
}