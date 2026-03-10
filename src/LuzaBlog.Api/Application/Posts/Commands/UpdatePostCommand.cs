using LuzaBlog.Api.Application.Common.Exceptions;
using LuzaBlog.Api.Application.Common.Interfaces;
using LuzaBlog.Api.Domain.Interfaces;
using MediatR;

namespace LuzaBlog.Api.Application.Posts.Commands;

public sealed record UpdatePostCommand(Guid PostId, string Title, string Content, string RequestingUserId) : IRequest;

public sealed class UpdatePostCommandHandler(
    IPostRepository postRepository,
    IApplicationDbContext dbContext) : IRequestHandler<UpdatePostCommand>
{
    public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(request.PostId, cancellationToken)
            ?? throw new NotFoundException($"Post {request.PostId} not found.");

        post.Update(request.Title, request.Content, request.RequestingUserId);

        await postRepository.UpdateAsync(post, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
