using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    public class CreateNotificationValidator : AbstractValidator<notification>, INotificationValidator
    {
        public CreateNotificationValidator()
        {
            RuleFor(x => x.userid)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.note)
                .NotEmpty().WithMessage("Note is required");
        }

        ValidationResult INotificationValidator.Validate(notification notification)
        {
            return this.Validate(notification);
        }

        Task<ValidationResult> INotificationValidator.ValidateAsync(notification notification)
        {
            return this.ValidateAsync(notification);
        }
    }
}