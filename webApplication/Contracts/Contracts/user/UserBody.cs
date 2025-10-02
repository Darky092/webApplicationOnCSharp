namespace Client.Components.bodies
{
    public class User
    {
        public int userid { get; set; }
        public string? avatar { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string? patronymic { get; set; }
        public string email { get; set; }
        public string telephonnumber { get; set; }
        public string passwordhash { get; set; }
        public string role { get; set; } 
        public bool? isactive { get; set; }
        public DateTimeOffset? createdat { get; set; }
    }
    public class UserReq
    {
        public string password { get; set; }
        public string email { get; set; }
    }
}
