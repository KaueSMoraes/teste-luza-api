using LuzaBlog.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuzaBlog.Api.Infrastructure.Persistence.Configurations;

public sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.OwnsOne(p => p.Title, b =>
        {
            b.Property(t => t.Value)
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(200);
        });

        builder.OwnsOne(p => p.Content, b =>
        {
            b.Property(c => c.Value)
                .HasColumnName("Content")
                .IsRequired()
                .HasMaxLength(10000);
        });

        builder.Property(p => p.AuthorId).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.DomainEvents);
    }
}
