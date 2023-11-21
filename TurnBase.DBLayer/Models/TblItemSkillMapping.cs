using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemSkillMapping
{
    public int ItemSkillId { get; set; }

    public int ItemId { get; set; }

    public int SkillId { get; set; }

    public int RowIndex { get; set; }

    public int ColIndex { get; set; }

    public virtual TblItem Item { get; set; } = null!;

    public virtual TblItemSkill Skill { get; set; } = null!;
}
