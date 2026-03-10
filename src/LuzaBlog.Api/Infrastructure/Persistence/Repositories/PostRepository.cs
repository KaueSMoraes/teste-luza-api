using LuzaBlog.Api.Domain.Entities;
using LuzaBlog.Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuzaBlog.Api.Infrastructure.Persistence.Repositories;

public sealed class PostRepository(ApplicationDbContext context) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken ct = default)
        => await context.Posts
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(Post post, CancellationToken ct = default)
        => await context.Posts.AddAsync(post, ct);

    public Task UpdateAsync(Post post, CancellationToken ct = default)
    {
        context.Posts.Update(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Post post, CancellationToken ct = default)
    {
        context.Posts.Remove(post);
        return Task.CompletedTask;
    }
}
