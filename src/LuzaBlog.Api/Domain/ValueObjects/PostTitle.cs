namespace LuzaBlog.Api.Domain.ValueObjects;

public sealed record PostTitle
{
    public string Value { get; }

    public PostTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty.", nameof(value));

        if (value.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters.", nameof(value));

        Value = value.Trim();
    }

    public static implicit operator string(PostTitle title) => title.Value;
    public static implicit operator PostTitle(string value) => new(value);
}
