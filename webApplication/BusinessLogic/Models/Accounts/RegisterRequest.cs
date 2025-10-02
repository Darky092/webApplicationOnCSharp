using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace BusinessLogic.Models.Accounts
{
    public class RegisterRequest
    {
        [Required] public string name { get; set; }
        [Required] public string surname { get; set; }
        public string? Patronymic { get; set; }
        [Required, EmailAddress] public string email { get; set; }
        [Required, MinLength(6)] public string passwordhash { get; set; }
        [Required, Compare("passwordhash")] public string ConfirmPassword { get; set; }
        [Phone] public string telephonnumber { get; set; }

        [Required] public string role { get; set; }
        [Range(typeof(bool), "true", "true")] public bool AcceptTerms { get; set; }
    }
}