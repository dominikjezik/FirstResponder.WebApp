using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Area("api")]
[IgnoreAntiforgeryToken]
[ApiController]
public abstract class ApiController : ControllerBase
{
}