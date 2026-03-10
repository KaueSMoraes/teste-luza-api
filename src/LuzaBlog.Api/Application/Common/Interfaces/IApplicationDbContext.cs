using LuzaBlog.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuzaBlog.Api.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Post> Posts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
