using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBase.DTOLayer.Models
{
    public struct PingDTO
    {
        [JsonProperty("A")] public long SendTime { get; set; }
    }
}
