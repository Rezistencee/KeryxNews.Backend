using KeryxNews.Domain.Entities;
using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeryxNews.Infrastructure.Persistence.Configuration;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasOne<Article>()
            .WithMany()
            .HasForeignKey(c => c.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<AppIdentityUser>()
            .WithMany()
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}