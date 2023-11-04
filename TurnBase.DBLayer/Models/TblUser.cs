using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblUser
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public Guid Token { get; set; }

    public int UserLevel { get; set; }

    public int Experience { get; set; }

    public long Gold { get; set; }
}
