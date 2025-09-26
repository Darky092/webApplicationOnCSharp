using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    public class CreateLectureValidator : AbstractValidator<lecture>, ILectureValidator
    {
        public CreateLectureValidator()
        {
            RuleFor(x => x.lecturename)
                .NotEmpty().WithMessage("Lecture name is required");

            RuleFor(x => x.teacherid)
                .GreaterThan(0).WithMessage("Teacher ID is required and must be greater than 0");
        }

        ValidationResult ILectureValidator.Validate(lecture lecture)
        {
            return this.Validate(lecture);
        }

        Task<ValidationResult> ILectureValidator.ValidateAsync(lecture lecture)
        {
            return this.ValidateAsync(lecture);
        }
    }
}