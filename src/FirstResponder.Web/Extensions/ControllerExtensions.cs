using FirstResponder.ApplicationCore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Extensions;

public static class ControllerExtensions
{
    public static void MapErrorsToModelState(this Controller controller, EntityValidationException exception)
    {
        if (exception.ValidationErrors != null)
        {
            foreach (var error in exception.ValidationErrors)
            {
                controller.ModelState.AddModelError(error.Key, error.Value);
            }
        }
        else
        {
            controller.ModelState.AddModelError(string.Empty, exception.Message);
        }
    }
    
    public static void MapErrorsToErrorMessages(this Controller controller, EntityValidationException exception)
    {
        if (exception.ValidationErrors != null)
        {
            controller.TempData["ErrorMessages"] = exception.ValidationErrors.Select(err => err.Value).ToArray();
        }
    }
    
    public static void DisplaySuccessMessage(this Controller controller, string message)
    {
        controller.TempData["SuccessMessage"] = message;
    }
    
    public static void DisplayErrorMessage(this Controller controller, string message)
    {
        controller.TempData["ErrorMessages"] = new[] { message };
    }
}