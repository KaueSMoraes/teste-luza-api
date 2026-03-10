using LuzaBlog.Api.Domain.Entities;
using LuzaBlog.Api.Domain.Interfaces;
using LuzaBlog.Api.Application.Common.Interfaces;
using MediatR;

namespace LuzaBlog.Api.Application.Posts.Commands;

public sealed record CreatePostCommand(string Title, string Content, string AuthorId) : IRequest<Guid>;

public sealed class CreatePostCommandHandler(
    IPostRepository postRepository,
    IApplicationDbContext dbContext,
    IMediator mediator) : IRequestHandler<CreatePostCommand, Guid>
{
    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = Post.Create(request.Title, request.Content, request.AuthorId);

        await postRepository.AddAsync(post, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in post.DomainEvents)
            await mediator.Publish(domainEvent, cancellationToken);

        post.ClearDomainEvents();

        return post.Id;
    }
}
