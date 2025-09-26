using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class group
{
    public int groupid { get; set; }

    public string groupname { get; set; } = null!;

    public int? course { get; set; }

    public int curatorid { get; set; }

    public string? specialty { get; set; }

    public int institutionid { get; set; }

    public virtual user curator { get; set; } = null!;

    public virtual institution institution { get; set; } = null!;

    public virtual ICollection<lecture> lectures { get; set; } = new List<lecture>();
}