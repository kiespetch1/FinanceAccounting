using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers.Abstractions;

/// <summary>
/// Base controller.
/// </summary>
public abstract class BaseController : ControllerBase
{ 
    /// <summary>
    /// Gets the id of the current user.
    /// </summary>
    /// <returns>Current user ID.</returns>
    protected int GetUserId()
    {
        return Convert.ToInt32(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}