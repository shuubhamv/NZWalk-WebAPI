using FluentValidation;
using NZWalk.Api.Models.DTO;

public class UpdateRegionRequestValidator : AbstractValidator<UpdateRegionRequestDto>
{
    public UpdateRegionRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .Length(3).WithMessage("Code must be exactly 3 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be a maximum of 100 characters.");
    }
}
