using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblProperty
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblItemProperty> TblItemProperties { get; set; } = new List<TblItemProperty>();
}
