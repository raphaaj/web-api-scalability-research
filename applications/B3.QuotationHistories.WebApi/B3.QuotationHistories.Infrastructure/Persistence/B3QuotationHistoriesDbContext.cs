using B3.QuotationHistories.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace B3.QuotationHistories.Infrastructure.Persistence;

public class B3QuotationHistoriesDbContext : DbContext
{
    public DbSet<QuotationHistory> QuotationHistories { get; set; }
    
    public DbSet<AssetAggregation> AssetsAggregations { get; set; }

    public B3QuotationHistoriesDbContext(DbContextOptions<B3QuotationHistoriesDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AssetAggregation>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("assets_aggregation");
        });
    }
}