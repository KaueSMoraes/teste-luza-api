using LuzaBlog.Api.Domain.Entities;

namespace LuzaBlog.Api.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}
