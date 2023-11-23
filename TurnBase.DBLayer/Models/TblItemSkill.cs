using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemSkill
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool FinalizeTurnInUse { get; set; }

    public int TurnCooldown { get; set; }

    public int UsageManaCost { get; set; }

    public virtual ICollection<TblItemSkillDataMapping> TblItemSkillDataMappings { get; set; } = new List<TblItemSkillDataMapping>();

    public virtual ICollection<TblItemSkillMapping> TblItemSkillMappings { get; set; } = new List<TblItemSkillMapping>();
}
