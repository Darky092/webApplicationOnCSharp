using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class user
{
    public int userid { get; set; }

    public string? avatar { get; set; }

    public string name { get; set; } = null!;

    public string? surname { get; set; }

    public string? patronymic { get; set; }

    public string email { get; set; } = null!;

    public string telephonnumber { get; set; } = null!;

    public string passwordhash { get; set; } = null!;

    public string role { get; set; } = null!;

    public bool? isactive { get; set; }

    public DateTime? createdat { get; set; }

    public virtual ICollection<attendance> attendances { get; set; } = new List<attendance>();

    public virtual ICollection<group> groups { get; set; } = new List<group>();

    public virtual ICollection<lecture> lectures { get; set; } = new List<lecture>();

    public virtual ICollection<notification> notifications { get; set; } = new List<notification>();

    public virtual ICollection<portfolio> portfolios { get; set; } = new List<portfolio>();
}