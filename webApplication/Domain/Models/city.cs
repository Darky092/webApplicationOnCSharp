using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class city
    {
    public int cityid { get; set; }

    public string cityname { get; set; } = null!;

    public string? postalcode { get; set; }

    public string? country { get; set; }

    public virtual ICollection<institution> institutions { get; set; } = new List<institution>();
    }