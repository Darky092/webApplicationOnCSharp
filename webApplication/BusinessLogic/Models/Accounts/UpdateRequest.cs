using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace BusinessLogic.Models.Accounts
{
    public class UpdateRequest
    {
        public string? name { get; set; }
        public string? surname { get; set; }
        public string? patronymic { get; set; }
        [EmailAddress] public string? email { get; set; }
        [Phone] public string? telephonnumber { get; set; }
        [MinLength(6)] public string? passwordhash { get; set; }
        [Compare("Password")] public string? ConfirmPassword { get; set; }
        public RoleT? RoleT { get; set; }
        public string role { get; set; }
    }
}