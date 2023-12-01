using System;
using System.Collections.Generic;

namespace TurnBase.DBLayer.Models;

public partial class TblItemSkillDataMapping
{
    public int Id { get; set; }

    public int ItemSkillId { get; set; }

    public int ItemSkillDataId { get; set; }

    public double MinQualityValue { get; set; }

    public double MaxQualityValue { get; set; }

    public virtual TblItemSkill ItemSkill { get; set; } = null!;

    public virtual TblItemSkillDatum ItemSkillData { get; set; } = null!;
}
