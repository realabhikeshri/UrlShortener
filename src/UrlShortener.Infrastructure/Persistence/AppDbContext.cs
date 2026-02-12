using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext.
/// Lives in Infrastructure to avoid leaking persistence into Domain.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();
    public DbSet<UrlAnalytics> UrlAnalytics => Set<UrlAnalytics>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>(entity =>
        {
            entity.ToTable("ShortUrls");

            entity.HasKey(x => x.Id);

            // Fast lookup during redirects
            entity.HasIndex(x => x.ShortCode).IsUnique();

            // Idempotency safety
            entity.HasIndex(x => x.LongUrl).IsUnique();

            entity.Property(x => x.LongUrl)
                  .IsRequired();
        });

        modelBuilder.Entity<UrlAnalytics>(entity =>
        {
            entity.ToTable("UrlAnalytics");

            entity.HasKey(x => x.Id);

            // One analytics row per short URL
            entity.HasIndex(x => x.ShortUrlId).IsUnique();
        });
    }
}
