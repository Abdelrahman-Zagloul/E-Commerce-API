using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        protected ActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);
        }
        protected ActionResult HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return HandleProblem(result.Errors);

        }



        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            if (errors.All(x => x.ErrorType == ErrorType.Validation))
                return HandleValidationProblem(errors);
            return HandleSigleProblem(errors[0]);

        }
        private ActionResult HandleSigleProblem(Error error)
        {
            return Problem(
                title: error.Title,
                detail: error.Description,
                statusCode: GetStatusCodeFromErrorType(error.ErrorType)
            );
        }
        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errors)
                modelState.AddModelError(error.Title, error.Description);
            return ValidationProblem(modelState);
        }
        private int GetStatusCodeFromErrorType(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

    }
}
