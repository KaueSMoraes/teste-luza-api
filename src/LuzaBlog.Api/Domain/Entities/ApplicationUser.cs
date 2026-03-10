using Microsoft.AspNetCore.Identity;

namespace LuzaBlog.Api.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public ICollection<Post> Posts { get; } = new List<Post>();
}
