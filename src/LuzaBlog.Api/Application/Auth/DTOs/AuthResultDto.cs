namespace LuzaBlog.Api.Application.Auth.DTOs;

public sealed record AuthResultDto(string Token, string UserId, string Email, string UserName);
