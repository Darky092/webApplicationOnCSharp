using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace BusinessLogic.Models.Accounts
{
    public class CreateRequest
    {
        [Required] public string name { get; set; }        
        [Required] public string surname { get; set; }      
        public string? patronymic { get; set; }              
        [Required, EmailAddress] public string email { get; set; }
        [Required, MinLength(6)] public string passwordhash { get; set; }
        [Required, Compare("Password")] public string confirmPassword { get; set; }
        public RoleT? RoleT { get; set; }

        [JsonPropertyName("role")] 
        public string role { get; set; }
    }
}