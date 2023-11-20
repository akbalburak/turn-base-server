using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblParameter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double? FloatValue { get; set; }

    public int? IntValue { get; set; }

    public bool? BoolValue { get; set; }

    public bool SendToClient { get; set; }
}
