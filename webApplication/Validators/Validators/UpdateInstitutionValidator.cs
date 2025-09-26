using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class UpdateInstitutionValidator : AbstractValidator<institution>, IInstitutionValidator
    {
        public UpdateInstitutionValidator()
        {
            
            RuleFor(x => x.institutionname)
                .MaximumLength(256).WithMessage("Institution name cannot exceed 256 characters");

            RuleFor(x => x.street)
                .MaximumLength(256).WithMessage("Street cannot exceed 256 characters");

            RuleFor(x => x.phone)
                .MaximumLength(50).WithMessage("Phone cannot exceed 50 characters");

            RuleFor(x => x.website)
                .MaximumLength(256).WithMessage("Website cannot exceed 256 characters");

            RuleFor(x => x.cityid)
                .GreaterThan(0).When(x => x.cityid.HasValue)
                .WithMessage("City ID must be greater than 0 if provided");
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