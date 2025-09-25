using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation.Results;

namespace Validators.Interefaces
{
    public interface IAttendanceValidator
    {
        ValidationResult Validate(attendance attendance);
        Task<ValidationResult> ValidateAsync(attendance attendance);
    }
}
