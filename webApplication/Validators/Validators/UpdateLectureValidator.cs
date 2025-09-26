using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class UpdateLectureValidator : AbstractValidator<lecture>, ILectureValidator
    {
        public UpdateLectureValidator()
        {
            RuleFor(x => x.lecturename)
                .NotEmpty().WithMessage("Lecture name is required");

            RuleFor(x => x.teacherid)
                .GreaterThan(0).WithMessage("Teacher ID is required and must be greater than 0");

        }
        //test actions

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