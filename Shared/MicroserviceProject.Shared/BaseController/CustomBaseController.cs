using MicroserviceProject.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceProject.Shared.BaseController;

[Route("api/[controller]")]
[ApiController]
public class CustomBaseController : ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult<T>(CustomResponse<T> response)
    {
        if (response.StatusCode == 204)
            return new ObjectResult(null)
            {
                StatusCode = response.StatusCode
            };

        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}