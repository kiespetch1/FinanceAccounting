using System.IdentityModel.Tokens.Jwt;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using Microsoft.AspNetCore.Mvc;


namespace FinanceAccounting.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    ///<summary/>
    private readonly IAuthService _authService;

    ///<summary/>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Registers a user.
    /// </summary>
    /// <param name="user">User registration data.</param>
    /// <returns>Status Code 200 (OK).</returns>
    /// <response code="200">Registration completed successfully.</response>
    /// <response code="400">Login or email is already taken.</response>
    [Route("registration")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegistrationData user)
    {
        await _authService.Register(user);
        
        return Ok();
    }
    
    /// <summary>
    /// Returns a JWT authorization token.
    /// </summary>
    /// <param name="authData">Email and password of the user.</param>
    /// <returns>User JWT token</returns>
    /// <response code="200">User successfully logged in.</response>
    /// <response code="400">Credentials are incorrect.</response>
    [Route("login")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult Login([FromBody] AuthData authData)
    {
        var jwt  = _authService.Login(authData);
        
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
}