using Domain.Models;
using FluentValidation;
using Validators.Interefaces;
using FluentValidation.Results;

namespace Validators.Validators
{
    public class CreateGroupValidator : AbstractValidator<group>, IGroupValidator
    {
        public CreateGroupValidator()
        {
            RuleFor(x => x.groupname)
                .NotEmpty().WithMessage("Group name is required");

            RuleFor(x => x.curatorid)
                .GreaterThan(0).WithMessage("Curator ID is required and must be greater than 0");

            RuleFor(x => x.institutionid)
                .GreaterThan(0).WithMessage("Institution ID is required and must be greater than 0");
        }

        ValidationResult IGroupValidator.Validate(group group)
        {
            return this.Validate(group);
        }

        Task<ValidationResult> IGroupValidator.ValidateAsync(group group)
        {
            return this.ValidateAsync(group);
        }
    }
}