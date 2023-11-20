using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.DTO
{
    public class ParameterDTO
    {
        public Parameters Id { get; set; }

        public string Name { get; set; } = null!;

        public double? FloatValue { get; set; }

        public int? IntValue { get; set; }

        public bool? BoolValue { get; set; }

        public bool SendToClient { get; set; }
    }

}
