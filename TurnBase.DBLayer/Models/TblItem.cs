using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeId { get; set; }

    public virtual ICollection<TblItemContentMapping> TblItemContentMappings { get; set; } = new List<TblItemContentMapping>();

    public virtual ICollection<TblItemPropertyMapping> TblItemPropertyMappings { get; set; } = new List<TblItemPropertyMapping>();

    public virtual ICollection<TblItemSkillMapping> TblItemSkillMappings { get; set; } = new List<TblItemSkillMapping>();

    public virtual TblItemType Type { get; set; } = null!;
}
