using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    internal class UpdateUserValidator : AbstractValidator<user>, IUserValidator
    {
        public UpdateUserValidator()
        {

            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100);

            RuleFor(x => x.surname)
                .NotEmpty().WithMessage("Surname is required")
                .MaximumLength(100);

            RuleFor(x => x.patronymic)
                .MaximumLength(100)
                .WithMessage("Patronymic is required");

            RuleFor(x => x.email)
                .NotEmpty().EmailAddress().MaximumLength(255).WithMessage("Incorrect email");

            RuleFor(x => x.passwordhash)
                .NotEmpty().MaximumLength(255).WithMessage("Password is required");

            RuleFor(x => x.role)
                .NotEmpty()
                .Must(role => role == "Student" || role == "Teacher" || role == "Admin")
                .WithMessage("Role must be one of: Student, Teacher, Admin");

            RuleFor(x => x.avatar)
                .NotEmpty().MaximumLength(500).WithMessage("Patronymic is required");

            RuleFor(x => x.telephonnumber)
                .NotEmpty()
                .Matches(@"^((8|\+7)[\- ]?)?(\(?[0-9]{3}\)?[\- ]?)?[0-9\- ]{7,10}$")
                .MaximumLength(20)
                .WithMessage("Format 89022372378");
            RuleFor(x => x.userid).NotEmpty().WithMessage("Name is required");
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