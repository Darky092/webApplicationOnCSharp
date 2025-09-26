using System.Threading.Tasks;
using Domain.Models;
using FluentValidation.Results;

namespace Validators.Interefaces
{
    public interface IPortfolioValidator
    {
        ValidationResult Validate(portfolio portfolio);
        Task<ValidationResult> ValidateAsync(portfolio portfolio);
    }
}