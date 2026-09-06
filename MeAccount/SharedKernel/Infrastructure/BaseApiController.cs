using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeAccount.SharedKernel.Infrastructure;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected Guid GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guid))
            throw new UnauthorizedAccessException("User ID not found or invalid in token");
        return guid;
    }
}
