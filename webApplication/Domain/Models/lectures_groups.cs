using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public partial class lectures_group
    {
        public int groupid { get; set; }
        public int lectureid { get; set; }

        public virtual group group { get; set; } = null!;
        public virtual lecture lecture { get; set; } = null!;
    }

}