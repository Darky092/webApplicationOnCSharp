namespace webApplication.Contracts.attendance
    {
    public class CreateAttendanceRequest
        {
        public int lectureid { get; set; }

        public int userid { get; set; }

        public bool ispresent { get; set; }

        public string? note { get; set; }
        }
    }