using LuzaBlog.Api.Application.Common.Exceptions;
using LuzaBlog.Api.Application.Posts.DTOs;
using LuzaBlog.Api.Domain.Interfaces;
using MediatR;

namespace LuzaBlog.Api.Application.Posts.Queries;

public sealed record GetPostByIdQuery(Guid Id) : IRequest<PostDto>;

public sealed class GetPostByIdQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostByIdQuery, PostDto>
{
    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Post {request.Id} not found.");

        return new PostDto(
            post.Id,
            post.Title.Value,
            post.Content.Value,
            post.AuthorId,
            post.Author?.UserName ?? "Unknown",
            post.CreatedAt,
            post.UpdatedAt);
    }
}
