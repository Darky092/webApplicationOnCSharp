using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class lecture
{
    public int lectureid { get; set; }

    public string lecturename { get; set; } = null!;

    public string? description { get; set; }

    public TimeOnly? starttime { get; set; }

    public TimeOnly? endtime { get; set; }

    public int teacherid { get; set; }

    public int? roomid { get; set; }

    public bool? isactive { get; set; }

    public DateTime? createdat { get; set; }

    public virtual ICollection<attendance> attendances { get; set; } = new List<attendance>();

    public virtual room? room { get; set; }

    public virtual user teacher { get; set; } = null!;

    public virtual ICollection<group> groups { get; set; } = new List<group>();
    
}