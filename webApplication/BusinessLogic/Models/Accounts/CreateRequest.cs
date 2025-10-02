using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace BusinessLogic.Models.Accounts
{
    public class CreateRequest
    {
        [Required] public string Name { get; set; }        
        [Required] public string Surname { get; set; }      
        public string? Patronymic { get; set; }              
        [Required, EmailAddress] public string Email { get; set; }
        [Required, MinLength(6)] public string Password { get; set; }
        [Required, Compare("Password")] public string ConfirmPassword { get; set; }
        public RoleT? RoleT { get; set; }

        [JsonPropertyName("role")] 
        public string role { get; set; }
    }
}