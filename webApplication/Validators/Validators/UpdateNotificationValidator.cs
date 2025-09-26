using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    public class UpdateNotificationValidator : AbstractValidator<notification>, INotificationValidator
    {
        public UpdateNotificationValidator()
        {
            RuleFor(x => x.userid)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.note)
                .NotEmpty().WithMessage("Note is required");

            RuleFor(x => x.isread)
                .NotNull().WithMessage("IsRead is required");
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