using LuzaBlog.Api.Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LuzaBlog.Api.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterCommand(request.Email, request.Password, request.UserName), ct);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(request.Email, request.Password), ct);
        return Ok(result);
    }
}

public sealed record RegisterRequest(string Email, string Password, string UserName);
public sealed record LoginRequest(string Email, string Password);
