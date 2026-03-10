using LuzaBlog.Api.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace LuzaBlog.Api.Infrastructure.Hubs;

public sealed class PostPublishedEventHandler(IHubContext<PostHub> hubContext)
    : INotificationHandler<PostPublishedEvent>
{
    public async Task Handle(PostPublishedEvent notification, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.SendAsync(
            "PostPublished",
            new
            {
                postId = notification.PostId,
                title = notification.Title,
                authorId = notification.AuthorId
            },
            cancellationToken);
    }
}
