using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Domain.Entities;
using B3.QuotationHistories.Domain.ValueObjects;
using B3.QuotationHistories.Infrastructure.Persistence;
using B3.QuotationHistories.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace B3.QuotationHistories.Infrastructure.Repositories;

public class QuotationHistoryRepository : IQuotationHistoryRepository
{
    private readonly B3QuotationHistoriesDbContext _context;

    public QuotationHistoryRepository(B3QuotationHistoriesDbContext context)
    {
        _context = context;
    }

    public async Task<QuotationHistoryEntity[]> GetQuotationHistoriesByPaperNegotiationCodeAsync(
        PaperNegotiationCode paperNegotiationCode)
    {
        var query = _context.QuotationHistories
            .AsNoTracking()
            .Where(x => x.PaperNegotiationCode == paperNegotiationCode.Value)
            .OrderBy(x => x.NegotiationDate)
            .ThenBy(x => x.Id);

        var quotationHistories = await query
            .Select(x => QuotationHistoryModelMapper.ToQuotationHistoryEntity(x))
            .ToArrayAsync();

        return quotationHistories;
    }

    public Task<TopAssetWithHighestNegotiatedVolumeDto[]> GetTopNAssetsWithHighestNegotiatedVolumeAsync(int n)
    {
        var query = _context.AssetsAggregations
            .OrderByDescending(x => x.TotalVolumeOfTitlesNegotiated)
            .ThenBy(x => x.PaperNegotiationCode)
            .Select(x =>
                new TopAssetWithHighestNegotiatedVolumeDto(x.PaperNegotiationCode, x.TotalVolumeOfTitlesNegotiated))
            .Take(n);

        return query.ToArrayAsync();
    }
}