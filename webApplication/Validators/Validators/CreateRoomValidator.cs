using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    public class CreateRoomValidator : AbstractValidator<room>, IRoomValidator
    {
        public CreateRoomValidator()
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