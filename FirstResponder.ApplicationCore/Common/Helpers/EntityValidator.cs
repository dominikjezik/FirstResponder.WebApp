using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Exceptions;

namespace FirstResponder.ApplicationCore.Common.Helpers;

public static class EntityValidator
{
    public static void Validate(object entity)
    {
        var context = new ValidationContext(entity);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(entity, context, validationResults, true);

        if (!isValid)
        {
            var errors = new Dictionary<string, string>();

            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    errors[memberName] = validationResult.ErrorMessage;
                }
            }
            
            throw new EntityValidationException(errors);
        }
    }
}