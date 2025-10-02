using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webApplication.Contracts.attendance
{
    public class AttendanceDetailsDto
    {
        public int AttendanceId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string LectureName { get; set; } = string.Empty;
        public DateTime? LectureCreatedAt { get; set; }
        public bool? IsPresent { get; set; }
        public string? Note { get; set; }
        public DateTime? RecordedAt { get; set; }
    }
}
