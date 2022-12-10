﻿using System.IdentityModel.Tokens.Jwt;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Models;
using Microsoft.AspNetCore.Mvc;
using static FinanceAccounting.Services.AuthService;

namespace FinanceAccounting.Controllers;



[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IRegistrationService _registrationService;

    public AuthController(ILoginService loginService, IRegistrationService registrationService)
    {
        _loginService = loginService;
        _registrationService = registrationService;
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
        var jwt  = _loginService.Login(authData);
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
        _registrationService.Register(user);
        return Ok();
    }
}