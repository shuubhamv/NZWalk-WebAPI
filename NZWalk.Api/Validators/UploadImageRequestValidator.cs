using FluentValidation;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.Validators
{
    public class UploadImageRequestValidator : AbstractValidator<ImageUploadRequestDto>
    {
        public UploadImageRequestValidator()
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(file => allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage("Invalid file type. Allowed types: .jpg, .jpeg, .png")
            .Must(file => file.Length <= 10485760)
            .WithMessage("File size should not exceed 10MB");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.");
            
        }
    }
}
