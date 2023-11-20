using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemContent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblItemContentMapping> TblItemContentMappings { get; set; } = new List<TblItemContentMapping>();
}
