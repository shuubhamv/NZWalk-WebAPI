using FluentValidation;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.Validators
{
    public class AddRegionRequestValidator:AbstractValidator<AddRegionRequestDto>
    {
        public AddRegionRequestValidator()
        {
                RuleFor(x => x.Code).NotEmpty()
                .MinimumLength(3).MaximumLength(3);

            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage(" Name is required");
        }
    }
}
