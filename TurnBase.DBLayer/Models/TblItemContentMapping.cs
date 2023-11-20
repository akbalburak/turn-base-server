using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemContentMapping
{
    public int ItemContentId { get; set; }

    public int ItemId { get; set; }

    public int ContentId { get; set; }

    public int? IndexId { get; set; }

    public double Value { get; set; }

    public virtual TblItemContent Content { get; set; } = null!;

    public virtual TblItem Item { get; set; } = null!;
}
