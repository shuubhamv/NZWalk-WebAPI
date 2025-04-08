using FluentValidation;
using NZWalk.Api.Models.DTO;

public class UpdateWalkRequestValidator : AbstractValidator<UpdateWalkRequestDto>
{
    public UpdateWalkRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be a maximum of 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must be a maximum of 500 characters.");

        RuleFor(x => x.LengthInKm)
            .InclusiveBetween(0, 50).WithMessage("Length must be between 0 and 50 km.");

        RuleFor(x => x.DifficultyId)
            .NotEmpty().WithMessage("Difficulty ID is required.");

        RuleFor(x => x.RegionId)
            .NotEmpty().WithMessage("Region ID is required.");
    }
}
