using LuzaBlog.Api.Application.Auth.DTOs;
using LuzaBlog.Api.Application.Common.Interfaces;
using LuzaBlog.Api.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LuzaBlog.Api.Application.Auth.Commands;

public sealed record RegisterCommand(string Email, string Password, string UserName) : IRequest<AuthResultDto>;

public sealed class RegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenService jwtTokenService) : IRequestHandler<RegisterCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        var token = jwtTokenService.GenerateToken(user);
        return new AuthResultDto(token, user.Id, user.Email!, user.UserName!);
    }
}
