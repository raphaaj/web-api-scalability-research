using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Domain.Entities;
using B3.QuotationHistories.Domain.ValueObjects;
using B3.QuotationHistories.Infrastructure.Persistence;
using B3.QuotationHistories.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace B3.QuotationHistories.Infrastructure.Repositories;

public class QuotationHistoryRepository : IQuotationHistoryRepository
{
    private readonly B3QuotationHistoriesDbContext _context;

    public QuotationHistoryRepository(B3QuotationHistoriesDbContext context)
    {
        _context = context;
    }

    public async Task<QuotationHistoryEntity[]> GetQuotationHistoriesAsync(
        QuotationHistoryFilterDto quotationHistoryFilter, PagingOptionsDto pagingOptions)
    {
        var query = BuildQueryForQuotationHistoriesAsync(quotationHistoryFilter, pagingOptions);

        return await query.Select(x => MapToEntity(x)).ToArrayAsync();
    }

    public Task<int> GetTotalNumberOfQuotationHistoriesAsync(QuotationHistoryFilterDto quotationHistoryFilter)
    {
        var query = BuildQueryForQuotationHistoriesAsync(quotationHistoryFilter);

        return query.CountAsync();
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

    private IQueryable<QuotationHistory> BuildQueryForQuotationHistoriesAsync(
        QuotationHistoryFilterDto quotationHistoryFilter, PagingOptionsDto pagingOptions)
    {
        var query = BuildQueryForQuotationHistoriesAsync(quotationHistoryFilter);

        query = query.OrderBy(x => x.NegotiationDate).ThenBy(x => x.Id);

        if (pagingOptions.Offset != null)
        {
            query = query.Skip(pagingOptions.Offset.Value);
        }

        if (pagingOptions.Limit != null)
        {
            query = query.Take(pagingOptions.Limit.Value);
        }

        return query;
    }

    private IQueryable<QuotationHistory> BuildQueryForQuotationHistoriesAsync(
        QuotationHistoryFilterDto quotationHistoryFilter)
    {
        var query = _context.QuotationHistories.AsNoTracking();

        if (quotationHistoryFilter.PaperNegotiationCode != null)
        {
            query = query.Where(x => x.PaperNegotiationCode == quotationHistoryFilter.PaperNegotiationCode.Value);
        }

        return query;
    }

    private static QuotationHistoryEntity MapToEntity(QuotationHistory quotationHistory)
    {
        return new QuotationHistoryEntity
        {
            Id = quotationHistory.Id,
            NegotiationDate = quotationHistory.NegotiationDate,
            BdiCode = quotationHistory.BdiCode,
            PaperNegotiationCode = quotationHistory.PaperNegotiationCode != null
                ? PaperNegotiationCode.Create(quotationHistory.PaperNegotiationCode)
                : null,
            MarketType = quotationHistory.MarketType,
            CompanyAbbreviatedName = quotationHistory.CompanyAbbreviatedName,
            PaperSpecification = quotationHistory.PaperSpecification,
            ForwardMarketTermInDays = quotationHistory.ForwardMarketTermInDays,
            ReferenceCurrency = quotationHistory.ReferenceCurrency,
            OpeningFloorPrice = quotationHistory.OpeningFloorPrice,
            HighestFloorPrice = quotationHistory.HighestFloorPrice,
            LowestFloorPrice = quotationHistory.LowestFloorPrice,
            AverageFloorPrice = quotationHistory.AverageFloorPrice,
            LastNegotiatedPrice = quotationHistory.LastNegotiatedPrice,
            BestPurchaseOfferPrice = quotationHistory.BestPurchaseOfferPrice,
            BestSalesOfferPrice = quotationHistory.BestSalesOfferPrice,
            NumberOfTradesConducted = quotationHistory.NumberOfTradesConducted,
            TotalQuantityOfTitlesTraded = quotationHistory.TotalQuantityOfTitlesTraded,
            TotalVolumeOfTitlesNegotiated = quotationHistory.TotalVolumeOfTitlesNegotiated,
            StrikePriceForOptionsMarket = quotationHistory.StrikePriceForOptionsMarket,
            CorrectionIndicatorForStrikePrice = quotationHistory.CorrectionIndicatorForStrikePrice,
            MaturityDateForOptionsMarket = quotationHistory.MaturityDateForOptionsMarket,
            PaperQuotationFactor = quotationHistory.PaperQuotationFactor,
            StrikePriceUsdPoints = quotationHistory.StrikePriceUsdPoints,
            IsinCode = quotationHistory.IsinCode,
            PaperDistributionNumber = quotationHistory.PaperDistributionNumber,
        };
    }
}