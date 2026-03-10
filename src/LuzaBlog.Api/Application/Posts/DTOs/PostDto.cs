namespace LuzaBlog.Api.Application.Posts.DTOs;

public sealed record PostDto(
    Guid Id,
    string Title,
    string Content,
    string AuthorId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
