using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation.Results;

namespace Validators.Interefaces
{
    public interface IRoomEquipmentValidator
    {
        ValidationResult Validate(room_equipment room_equipment);
        Task<ValidationResult> ValidateAsync(room_equipment room_equipment);
    }
}