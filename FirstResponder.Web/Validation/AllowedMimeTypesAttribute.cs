using System.ComponentModel.DataAnnotations;

namespace FirstResponder.Web.Validation;

public class AllowedMimeTypesAttribute : ValidationAttribute
{
    public string GetErrorMessage() =>
        $"Only files with type: {string.Join(", ", allowedMimeTypes)} are allowed.";

    private readonly string[] allowedMimeTypes;

    public AllowedMimeTypesAttribute(params string[] mimeTypes)
    {
        allowedMimeTypes = mimeTypes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (!allowedMimeTypes.Contains(file.ContentType))
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        return ValidationResult.Success;
    }

}