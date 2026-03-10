using LuzaBlog.Api.Application.Posts.DTOs;
using LuzaBlog.Api.Domain.Interfaces;
using MediatR;

namespace LuzaBlog.Api.Application.Posts.Queries;

public sealed record GetPostsQuery : IRequest<IReadOnlyList<PostDto>>;

public sealed class GetPostsQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostsQuery, IReadOnlyList<PostDto>>
{
    public async Task<IReadOnlyList<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await postRepository.GetAllAsync(cancellationToken);

        return posts
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PostDto(
                p.Id,
                p.Title.Value,
                p.Content.Value,
                p.AuthorId,
                p.Author?.UserName ?? "Unknown",
                p.CreatedAt,
                p.UpdatedAt))
            .ToList();
    }
}
