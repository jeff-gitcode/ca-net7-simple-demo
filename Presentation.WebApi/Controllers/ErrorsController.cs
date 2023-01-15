using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers
{
    [AllowAnonymous] // Don't apply authorization to this controller
    public class ErrorsController : ApiController
    {
        // [Route("error")]
        [HttpGet]
        public async Task<IActionResult> Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error ?? new Exception("Unknown error");
            return Problem(title: exception.Message, statusCode: 400);
        }
    }
}