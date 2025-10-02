namespace Client.Components.bodies
{

        public class Lecture
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

 
    }
}
