using B3.QuotationHistories.Importer.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace B3.QuotationHistories.Importer.Database;

public class B3QuotationHistoriesDbContext(DbContextOptions<B3QuotationHistoriesDbContext> options)
    : DbContext(options)
{
    public DbSet<QuotationHistory> QuotationHistories { get; set; }
}