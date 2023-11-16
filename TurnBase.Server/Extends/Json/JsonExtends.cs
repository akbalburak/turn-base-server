using Newtonsoft.Json;
using System.Text;
using TurnBase.Server.Server;

namespace TurnBase.Server.Extends.Json
{
    public static class JsonExtends
    {
        public static T ToObject<T>(this object source)
        {
            return JsonConvert.DeserializeObject<T>($"{source}");
        }

        public static T ToObject<T>(this object source, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>($"{source}", settings);
        }

        public static string ToJson<T>(this T source)
        {
            return JsonConvert.SerializeObject(source);
        }
        public static string ToJson<T>(this T source, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(source, settings);
        }

        public static byte[] ToByteArray(this object source)
        {
            string data = $"{JsonConvert.SerializeObject(source) + TcpServer.ENDFIX}";
            return Encoding.UTF8.GetBytes(data);
        }
    }
}
