using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace TurnBase.Server.Extends.Json.JsonContracts
{
    public class IgnoreJsonProperty : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = member.Name;
            return property;
        }
    }
}
