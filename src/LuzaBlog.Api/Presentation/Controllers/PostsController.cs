using System.Security.Claims;
using LuzaBlog.Api.Application.Posts.Commands;
using LuzaBlog.Api.Application.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuzaBlog.Api.Presentation.Controllers;

[ApiController]
[Route("api/posts")]
public sealed class PostsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var posts = await mediator.Send(new GetPostsQuery(), ct);
        return Ok(posts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var post = await mediator.Send(new GetPostByIdQuery(id), ct);
        return Ok(post);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request, CancellationToken ct)
    {
        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var id = await mediator.Send(new CreatePostCommand(request.Title, request.Content, authorId), ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostRequest request, CancellationToken ct)
    {
        var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await mediator.Send(new UpdatePostCommand(id, request.Title, request.Content, requestingUserId), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await mediator.Send(new DeletePostCommand(id, requestingUserId), ct);
        return NoContent();
    }
}

public sealed record CreatePostRequest(string Title, string Content);
public sealed record UpdatePostRequest(string Title, string Content);
