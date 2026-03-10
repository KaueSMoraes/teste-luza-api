using MediatR;

namespace LuzaBlog.Api.Domain.Events;

public sealed record PostPublishedEvent(Guid PostId, string Title, string AuthorId) : INotification;
