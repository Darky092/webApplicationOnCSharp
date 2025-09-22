namespace webApplication.Contracts.lecture
{
    public class CreateLectureRequest
    {
        

        public string lecturename { get; set; } = null!;

        public string? description { get; set; }

        public TimeOnly starttime { get; set; }

        public TimeOnly endtime { get; set; }

        public int teacherid { get; set; }

        public int? roomid { get; set; }


    }
}
