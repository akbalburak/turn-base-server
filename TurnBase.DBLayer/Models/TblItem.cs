using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeId { get; set; }
    
    public virtual ICollection<TblItemProperty> TblItemProperties { get; set; } = new List<TblItemProperty>();

    public virtual TblItemType Type { get; set; } = null!;
}
