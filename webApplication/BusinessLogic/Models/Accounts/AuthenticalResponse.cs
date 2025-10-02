using System.Text.Json.Serialization;

namespace BusinessLogic.Models.Accounts
{
    public class AuthenticateResponse
    {
        public int userid { get; set; }
        public string name { get; set; } = null!;
        public string surname { get; set; } = null!;
        public string? patronymic { get; set; }
        public string email { get; set; } = null!;
        public string role { get; set; } = null!;

        public string RoleT { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; } = null!;

        [JsonIgnore]
        public string RefreshToken { get; set; } = null!;
    }
}