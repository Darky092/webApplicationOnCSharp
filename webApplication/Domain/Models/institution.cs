using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class institution
    {
    public int institutionid { get; set; }

    public string institutionname { get; set; } = null!;

    public string street { get; set; } = null!;

    public string? phone { get; set; }

    public string? website { get; set; }

    public int? cityid { get; set; }

    public virtual city? cityNavigation { get; set; }

    public virtual ICollection<group> groups { get; set; } = new List<group>();

    public virtual ICollection<room> rooms { get; set; } = new List<room>();
    }