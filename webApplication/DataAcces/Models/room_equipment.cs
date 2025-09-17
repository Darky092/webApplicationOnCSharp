using System;
using System.Collections.Generic;

namespace DataAcces.Models;

public partial class room_equipment
{
    public int roomid { get; set; }

    public string equipment { get; set; } = null!;

    public virtual room room { get; set; } = null!;
}
