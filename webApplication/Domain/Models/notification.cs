using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class notification
    {
    public int notificationid { get; set; }

    public int userid { get; set; }

    public DateTime? createdat { get; set; }

    public bool? isread { get; set; }

    public string note { get; set; } = null!;

    public virtual user user { get; set; } = null!;
    }