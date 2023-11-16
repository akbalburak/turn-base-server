using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblContent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblItemContent> TblItemContents { get; set; } = new List<TblItemContent>();
}
