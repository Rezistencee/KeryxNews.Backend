using KeryxNews.Domain.Entities;
using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeryxNews.Infrastructure.Persistence.Configuration;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");
        
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(a => a.Content)
            .IsRequired();

        builder.Property(a => a.Views)
            .HasDefaultValue(0);
        
        builder.Property(a => a.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne<AppIdentityUser>()
            .WithMany()
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}