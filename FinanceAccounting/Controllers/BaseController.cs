using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers;

public abstract class BaseController : ControllerBase
{ 
    protected int GetUserId()
    {
        return Convert.ToInt32(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}