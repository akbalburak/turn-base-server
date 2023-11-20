using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemProperty
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblItemPropertyMapping> TblItemPropertyMappings { get; set; } = new List<TblItemPropertyMapping>();
}
