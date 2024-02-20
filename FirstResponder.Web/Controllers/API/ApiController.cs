using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[IgnoreAntiforgeryToken]
[ApiController]
public abstract class ApiController : ControllerBase
{
}