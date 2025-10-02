using System.Text.Json.Serialization;

namespace BusinessLogic.Models.Accounts
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; } = null!;

        [JsonIgnore]
        public string RefreshToken { get; set; } = null!;
    }
}