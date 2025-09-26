using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class CreateLecturGroupValidator : AbstractValidator<lectures_group>, ILectureGroupValidator
    {
        public CreateLecturGroupValidator()
        {
            RuleFor(x => x.groupid)
                .GreaterThan(0).WithMessage("Group ID is required and must be greater than 0");

            RuleFor(x => x.lectureid)
                .GreaterThan(0).WithMessage("Lecture ID is required and must be greater than 0");
        }

        ValidationResult ILectureGroupValidator.Validate(lectures_group lectures_group)
        {
            return this.Validate(lectures_group);
        }

        Task<ValidationResult> ILectureGroupValidator.ValidateAsync(lectures_group lectures_group)
        {
            return this.ValidateAsync(lectures_group);
        }
    }
}