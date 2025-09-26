using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    public class CreatePortfolioValidator : AbstractValidator<portfolio>, IPortfolioValidator
    {
        public CreatePortfolioValidator()
        {
            RuleFor(x => x.userid)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.achievement)
                .NotEmpty().WithMessage("Achievement is required");
        }

        ValidationResult IPortfolioValidator.Validate(portfolio portfolio)
        {
            return this.Validate(portfolio);
        }

        Task<ValidationResult> IPortfolioValidator.ValidateAsync(portfolio portfolio)
        {
            return this.ValidateAsync(portfolio);
        }
    }
}