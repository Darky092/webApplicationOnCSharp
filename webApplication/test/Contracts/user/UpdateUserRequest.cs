namespace webApplication.Contracts.user
{
    public class UpdateUserRequest
    {
        public int userid { get; set; }

        public string? avatar { get; set; }

        public string name { get; set; } = null!;

        public string? surname { get; set; }

        public string? patronymic { get; set; }

        public string email { get; set; } = null!;

        public string telephonnumber { get; set; } = null!;

        public string passwordhash { get; set; } = null!;

        public string role { get; set; } = null!;

        public bool? isactive { get; set; }

        public DateTime? createdat { get; set; }
    }
}
