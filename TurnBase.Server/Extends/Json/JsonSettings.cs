using Newtonsoft.Json;
using TurnBase.Server.Extends.Json.JsonContracts;

namespace TurnBase.Server.Extends.Json
{
    public static class JsonSettings
    {
        private static JsonSerializerSettings? _ignoreJsonPropertySetting;
        public static JsonSerializerSettings IgnoreJsonPropertySettings
        {
            get
            {
                _ignoreJsonPropertySetting ??= new JsonSerializerSettings()
                {
                    ContractResolver = new IgnoreJsonProperty(),
                    NullValueHandling = NullValueHandling.Ignore
                };

                return _ignoreJsonPropertySetting;
            }
        }
    }
}
