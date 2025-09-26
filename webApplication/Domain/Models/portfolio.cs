using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class portfolio
{
    public int userid { get; set; }

    public string achievement { get; set; } = null!;

    public DateTime? addedat { get; set; }

    public virtual user user { get; set; } = null!;
}