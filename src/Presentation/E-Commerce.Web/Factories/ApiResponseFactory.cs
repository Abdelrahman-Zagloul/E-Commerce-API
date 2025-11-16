using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Factories
{
    internal static class ApiResponseFactory
    {
        internal static IActionResult CreateValidationErrorResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Any() ?? false)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value?.Errors.Select(x => x.ErrorMessage).ToArray());


            var errorResponse = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred.",
                Detail = "See the errors property for more details.",
                Extensions = { { "errors", errors } },
                Instance = context.HttpContext.Request.Path
            };
            return new BadRequestObjectResult(errorResponse);
        }
    }
}
