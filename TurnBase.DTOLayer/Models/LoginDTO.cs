﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBase;
using TurnBase.DTOLayer;
using TurnBase.DTOLayer.Models;

namespace TurnBase.DTOLayer.Models
{
    public class LoginDTO
    {
        public class LoginRequestDTO
        {
            [JsonProperty("A")] public string Token { get; set; }
        }

        public class LoginResponseDTO
        {
            [JsonProperty("A")] public UserDTO User { get; set; }
        }
    }
}