using LuzaBlog.Api.Application.Common.Exceptions;
using LuzaBlog.Api.Application.Common.Interfaces;
using LuzaBlog.Api.Domain.Interfaces;
using MediatR;

namespace LuzaBlog.Api.Application.Posts.Commands;

public sealed record DeletePostCommand(Guid PostId, string RequestingUserId) : IRequest;

public sealed class DeletePostCommandHandler(
    IPostRepository postRepository,
    IApplicationDbContext dbContext) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(request.PostId, cancellationToken)
            ?? throw new NotFoundException($"Post {request.PostId} not found.");

        if (post.AuthorId != request.RequestingUserId)
            throw new ForbiddenException("You are not allowed to delete this post.");

        await postRepository.DeleteAsync(post, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
