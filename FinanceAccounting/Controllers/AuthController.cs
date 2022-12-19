using System.IdentityModel.Tokens.Jwt;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using FinanceAccounting.Services;
using Microsoft.AspNetCore.Mvc;


namespace FinanceAccounting.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="authData">Email and password of the user</param>
    /// <returns>User JWT Token, Status Code 200 (OK)</returns>
    /// <exception cref="WrongCredentialsException">Credentials are incorrect</exception>
    /// <response code="200">User successfully logged in</response>
    /// <response code="400">Credentials are incorrect</response>
    [Route("login")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult Login([FromBody] AuthData authData)
    {
        var jwt  = _authService.Login(authData);
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"> Login, name, email, middle name, last name, birth date and password of the user</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="ExistingLoginException">Login or email is already taken</exception>
    /// <response code="200">Registration completed successfully</response>
    /// <response code="400">Login or email is already taken</response>
    [Route("registration")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult Register([FromBody] RegistrationData user)
    {
        _authService.Register(user);
        return Ok();
    }
}