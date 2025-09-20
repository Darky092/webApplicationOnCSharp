using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class students_group
{
    public int userid { get; set; }

    public int groupid { get; set; }

    public DateOnly? enrolledat { get; set; }

    public virtual group group { get; set; } = null!;

    public virtual user user { get; set; } = null!;
}
