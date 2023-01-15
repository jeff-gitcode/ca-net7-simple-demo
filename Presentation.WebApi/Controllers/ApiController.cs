using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApiController : ControllerBase
{
    public ApiController()
    {
    }

}