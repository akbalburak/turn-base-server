using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemSkill
{
    public int ItemSkillId { get; set; }

    public int ItemId { get; set; }

    public int SkillId { get; set; }

    public int SlotIndex { get; set; }

    public virtual TblItem Item { get; set; } = null!;

    public virtual TblSkill Skill { get; set; } = null!;
}
