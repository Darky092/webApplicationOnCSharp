using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators
{
    public class CreateUserValidator : AbstractValidator<user>, IUserValidator
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100);

            RuleFor(x => x.surname)
                .NotEmpty().WithMessage("Surname is required")
                .MaximumLength(100);

            RuleFor(x => x.patronymic)
                .NotEmpty().WithMessage("Patronymic is required")
                .MaximumLength(100);

            RuleFor(x => x.email)
                .NotEmpty().EmailAddress().MaximumLength(255);

            RuleFor(x => x.passwordhash)
                .NotEmpty().MaximumLength(255);

            RuleFor(x => x.role)
                .NotEmpty()
                .Must(role => role == "Student" || role == "Teacher" || role == "Admin")
                .WithMessage("Role must be one of: Student, Teacher, Admin");


            RuleFor(x => x.telephonnumber)
                .NotEmpty()
                .Matches(@"^((8|\+7)[\- ]?)?(\(?[0-9]{3}\)?[\- ]?)?[0-9\- ]{7,10}$")
                .MaximumLength(20).WithMessage("Fromat 89067893040"); ;
        }


        ValidationResult IUserValidator.Validate(user user)
        {
            return this.Validate(user);
        }

        Task<ValidationResult> IUserValidator.ValidateAsync(user user)
        {
            return this.ValidateAsync(user, CancellationToken.None);
        }
    }
}