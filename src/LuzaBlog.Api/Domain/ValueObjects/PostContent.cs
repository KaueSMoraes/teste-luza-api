namespace LuzaBlog.Api.Domain.ValueObjects;

public sealed record PostContent
{
    public string Value { get; }

    public PostContent(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Content cannot be empty.", nameof(value));

        if (value.Length > 10000)
            throw new ArgumentException("Content cannot exceed 10000 characters.", nameof(value));

        Value = value.Trim();
    }

    public static implicit operator string(PostContent content) => content.Value;
    public static implicit operator PostContent(string value) => new(value);
}
