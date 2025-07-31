using Auctify.CatalogService.Application.Interfaces;
using Auctify.CatalogService.Domain;
using Microsoft.EntityFrameworkCore;

namespace Auctify.CatalogService.Infrastructure.Persistence;
internal sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options), IUnitOfWork {
    public DbSet<AuctionItem> AuctionItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AuctionItem>(entity => {
            entity.HasKey(ai => ai.Id);
            entity.Property(ai => ai.Id)
                .HasConversion(id => id.Value, value => AuctionItemId.From(value))
                .IsRequired();
            entity.Property(ai => ai.Title).IsRequired().HasMaxLength(200);
            entity.Property(ai => ai.StartingPrice).IsRequired();
            entity.Property(ai => ai.CurrentPrice);
            entity.Property(ai => ai.CreatedAt).IsRequired();
            entity.Property(ai => ai.UpdatedAt);
        });
        base.OnModelCreating(modelBuilder);
    }
}