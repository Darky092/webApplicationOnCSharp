using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace BusinessLogic.Models.Accounts
{
    public class UpdateRequest
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        [EmailAddress] public string? Email { get; set; }
        [Phone] public string? TelephoneNumber { get; set; }
        [MinLength(6)] public string? Password { get; set; }
        [Compare("Password")] public string? ConfirmPassword { get; set; }
        public RoleT? RoleT { get; set; }

        public string role { get; set; }
    }
}