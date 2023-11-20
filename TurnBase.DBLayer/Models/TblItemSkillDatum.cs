using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemSkillDatum
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblItemSkillDataMapping> TblItemSkillDataMappings { get; set; } = new List<TblItemSkillDataMapping>();
}
