using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Presentation.WebApi.Filter;

namespace Presentation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ErrorHandlingFilter]
[Authorize]
public class ApiController : ControllerBase
{
    public ApiController()
    {
    }

}