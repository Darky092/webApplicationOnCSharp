using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class attendance
{
    public int attendanceid { get; set; }

    public int lectureid { get; set; }

    public int userid { get; set; }

    public bool? ispresent { get; set; }

    public string? note { get; set; }

    public DateTime? recordedat { get; set; }

    public virtual lecture lecture { get; set; } = null!;

    public virtual user user { get; set; } = null!;
}