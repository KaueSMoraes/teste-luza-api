using LuzaBlog.Api.Application.Auth.DTOs;
using LuzaBlog.Api.Application.Common.Interfaces;
using LuzaBlog.Api.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LuzaBlog.Api.Application.Auth.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResultDto>;

public sealed class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        var isValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isValid)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = jwtTokenService.GenerateToken(user);
        return new AuthResultDto(token, user.Id, user.Email!, user.UserName!);
    }
}
