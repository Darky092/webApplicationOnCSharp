using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Validators.Interefaces;

namespace Validators.Validators
{
    public class CreateInstitutionValidator : AbstractValidator<institution>, IInstitutionValidator
    {
        public CreateInstitutionValidator()
        {
            RuleFor(x => x.institutionname)
                .NotEmpty().WithMessage("Institution name is required");

            RuleFor(x => x.street)
                .NotEmpty().WithMessage("Street is required");
        }

        ValidationResult IInstitutionValidator.Validate(institution institution)
        {
            return this.Validate(institution);
        }

        Task<ValidationResult> IInstitutionValidator.ValidateAsync(institution institution)
        {
            return this.ValidateAsync(institution);
        }
    }
}