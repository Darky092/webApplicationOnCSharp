using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation.Results;

namespace Validators.Interefaces
{
    public interface ILectureValidator
    {
        ValidationResult Validate(lecture lecture);
        Task<ValidationResult> ValidateAsync(lecture lecture);
    }
}
