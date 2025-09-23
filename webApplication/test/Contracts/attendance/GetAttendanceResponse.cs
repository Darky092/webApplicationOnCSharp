namespace webApplication.Contracts.attendance
    {
    public class GetAttendanceResponse
        {
        public int attendanceid { get; set; }

        public int lectureid { get; set; }

        public int userid { get; set; }

        public bool ispresent { get; set; }

        public string? note { get; set; }
        }
    }