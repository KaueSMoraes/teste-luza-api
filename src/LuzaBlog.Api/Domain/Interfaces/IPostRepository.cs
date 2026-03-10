using LuzaBlog.Api.Domain.Entities;

namespace LuzaBlog.Api.Domain.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Post post, CancellationToken ct = default);
    Task UpdateAsync(Post post, CancellationToken ct = default);
    Task DeleteAsync(Post post, CancellationToken ct = default);
}
