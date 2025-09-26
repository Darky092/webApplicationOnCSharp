using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class CreateCityValidator : AbstractValidator<city>, ICityValidator
    {
        public CreateCityValidator()
        {
            RuleFor(x => x.cityname)
                .NotEmpty().WithMessage("City name is required");
        }

        ValidationResult ICityValidator.Validate(city city)
        {
            return this.Validate(city);
        }

        Task<ValidationResult> ICityValidator.ValidateAsync(city city)
        {
            return this.ValidateAsync(city);
        }
    }
}