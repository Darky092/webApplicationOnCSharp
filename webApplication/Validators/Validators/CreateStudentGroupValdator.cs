using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class CreateStudentGroupValidator : AbstractValidator<students_group>, IStudentGroupValidator
    {
        public CreateStudentGroupValidator()
        {
            RuleFor(x => x.userid)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.groupid)
                .NotEmpty().WithMessage("GroupId is required");
        }

        ValidationResult IStudentGroupValidator.Validate(students_group students_group)
        {
            return this.Validate(students_group);
        }

        Task<ValidationResult> IStudentGroupValidator.ValidateAsync(students_group students_group)
        {
            return this.ValidateAsync(students_group);
        }
    }
}