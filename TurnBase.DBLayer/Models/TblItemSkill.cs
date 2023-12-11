using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemSkill
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool FinalizeTurnInUse { get; set; }

    public int ShapeId { get; set; }

    public int TargetId { get; set; }

    public bool IsCombatSkill { get; set; }

    public virtual TblItemSkillShape Shape { get; set; } = null!;

    public virtual TblItemSkillTarget Target { get; set; } = null!;

    public virtual ICollection<TblItemSkillDataMapping> TblItemSkillDataMappings { get; set; } = new List<TblItemSkillDataMapping>();

    public virtual ICollection<TblItemSkillMapping> TblItemSkillMappings { get; set; } = new List<TblItemSkillMapping>();
}
