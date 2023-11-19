using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblSkill
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool FinalizeTurnInUse { get; set; }

    public int TurnCooldown { get; set; }

    public virtual ICollection<TblItemSkill> TblItemSkills { get; set; } = new List<TblItemSkill>();
}
