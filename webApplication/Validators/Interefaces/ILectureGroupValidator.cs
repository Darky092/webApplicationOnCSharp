using System.Threading.Tasks;
using Domain.Models;
using FluentValidation.Results;

namespace Validators.Interefaces
{
    public interface ILectureGroupValidator
    {
        ValidationResult Validate(lectures_group lectures_group);
        Task<ValidationResult> ValidateAsync(lectures_group lectures_group);
    }
}