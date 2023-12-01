using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemSkillTarget
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblItemSkill> TblItemSkills { get; set; } = new List<TblItemSkill>();
}
