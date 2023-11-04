using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBase.DTOLayer.Models
{
    public class UserLevelDTO
    {
        [JsonProperty("A")] public int Level { get; set; }
        [JsonProperty("B")] public int Experience { get; set; }
    }
}
