using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DynamicPortfolioSite.Api.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [HttpGet("error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = 500; // Internal Server Error (default)

            if (exception is NullReferenceException) code = 404; // Not Found
            else if (exception is UnauthorizedAccessException) code = 401; // Unauthorized

            return StatusCode(code, exception.GetType().FullName); 
        }
    }
}
