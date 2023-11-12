using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemProperty
{
    public int ItemPropertyId { get; set; }

    public int ItemId { get; set; }

    public int PropertyId { get; set; }

    public double MinValue { get; set; }

    public double MaxValue { get; set; }

    public virtual TblItem Item { get; set; } = null!;

    public virtual TblProperty Property { get; set; } = null!;
}
