using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation.Results;

namespace Validators.Interefaces
{
    public interface IRoomValidator
    {
        ValidationResult Validate(room room);
        Task<ValidationResult> ValidateAsync(room room);
    }
}