using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class room
    {
    public int roomid { get; set; }

    public string roomnumber { get; set; } = null!;

    public int institutionid { get; set; }

    public virtual institution institution { get; set; } = null!;

    public virtual ICollection<lecture> lectures { get; set; } = new List<lecture>();

    public virtual ICollection<room_equipment> room_equipments { get; set; } = new List<room_equipment>();
    }