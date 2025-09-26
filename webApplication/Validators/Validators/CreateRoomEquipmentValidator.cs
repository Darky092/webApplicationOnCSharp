using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class CreateRoomEquipmentValidator : AbstractValidator<room_equipment>, IRoomEquipmentValidator
    {
        public CreateRoomEquipmentValidator()
        {
            RuleFor(x => x.roomid)
                .NotEmpty().WithMessage("Room ID is required");

            RuleFor(x => x.equipment)
                .NotEmpty().WithMessage("Equipment name is required");
        }

        ValidationResult IRoomEquipmentValidator.Validate(room_equipment room_equipment)
        {
            return this.Validate(room_equipment);
        }

        Task<ValidationResult> IRoomEquipmentValidator.ValidateAsync(room_equipment room_equipment)
        {
            return this.ValidateAsync(room_equipment);
        }
    }
}