using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    public class CreateAttendanceValidator : AbstractValidator<attendance>, IAttendanceValidator
    {
        public CreateAttendanceValidator()
        {
            RuleFor(x => x.lectureid)
                .GreaterThan(0).WithMessage("Lecture ID is required and must be greater than 0");

            RuleFor(x => x.userid)
                .GreaterThan(0).WithMessage("User ID is required and must be greater than 0");
        }

        ValidationResult IAttendanceValidator.Validate(attendance attendance)
        {
            return this.Validate(attendance);
        }

        Task<ValidationResult> IAttendanceValidator.ValidateAsync(attendance attendance)
        {
            return this.ValidateAsync(attendance);
        }
    }
}