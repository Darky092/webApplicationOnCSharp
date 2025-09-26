using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class UpdateRoomValidator : AbstractValidator<room>, IRoomValidator
    {
        public UpdateRoomValidator()
        {
            RuleFor(x => x.roomnumber)
                .NotEmpty().WithMessage("Room number is required");

            RuleFor(x => x.institutionid)
                .NotEmpty().WithMessage("Institution ID is required");
        }

        ValidationResult IRoomValidator.Validate(room room)
        {
            return this.Validate(room);
        }

        Task<ValidationResult> IRoomValidator.ValidateAsync(room room)
        {
            return this.ValidateAsync(room);
        }
    }
}