using FirstResponder.ApplicationCore.Exceptions;
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
}