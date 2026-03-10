using LuzaBlog.Api.Application.Common.Exceptions;
using LuzaBlog.Api.Domain.Events;
using LuzaBlog.Api.Domain.ValueObjects;
using MediatR;

namespace LuzaBlog.Api.Domain.Entities;

public sealed class Post
{
    private readonly List<INotification> _domainEvents = [];

    private Post() { }

    public Guid Id { get; private set; }
    public PostTitle Title { get; private set; } = null!;
    public PostContent Content { get; private set; } = null!;
    public string AuthorId { get; private set; } = null!;
    public ApplicationUser Author { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyList<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public static Post Create(string title, string content, string authorId)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = new PostTitle(title),
            Content = new PostContent(content),
            AuthorId = authorId,
            CreatedAt = DateTime.UtcNow
        };

        post._domainEvents.Add(new PostPublishedEvent(post.Id, post.Title.Value, post.AuthorId));

        return post;
    }

    public void Update(string title, string content, string requestingUserId)
    {
        if (AuthorId != requestingUserId)
            throw new ForbiddenException("You are not allowed to edit this post.");

        Title = new PostTitle(title);
        Content = new PostContent(content);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
