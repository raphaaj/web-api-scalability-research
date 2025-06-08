using B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;
using B3.QuotationHistories.Domain.Entities;

namespace B3.QuotationHistories.Application.Mappers;

public static class QuotationHistoryEntityMapper
{
    public static PaperQuotationHistoryDto ToPaperHistoricalQuotationDto(
        QuotationHistoryEntity quotationHistoryEntity)
    {
        return new PaperQuotationHistoryDto
        {
            Id = quotationHistoryEntity.Id,
            NegotiationDate = quotationHistoryEntity.NegotiationDate,
            BdiCode = quotationHistoryEntity.BdiCode,
            PaperNegotiationCode = quotationHistoryEntity.PaperNegotiationCode?.Value,
            MarketType = quotationHistoryEntity.MarketType,
            CompanyAbbreviatedName = quotationHistoryEntity.CompanyAbbreviatedName,
            PaperSpecification = quotationHistoryEntity.PaperSpecification,
            ForwardMarketTermInDays = quotationHistoryEntity.ForwardMarketTermInDays,
            ReferenceCurrency = quotationHistoryEntity.ReferenceCurrency,
            OpeningFloorPrice = quotationHistoryEntity.OpeningFloorPrice,
            HighestFloorPrice = quotationHistoryEntity.HighestFloorPrice,
            LowestFloorPrice = quotationHistoryEntity.LowestFloorPrice,
            AverageFloorPrice = quotationHistoryEntity.AverageFloorPrice,
            LastNegotiatedPrice = quotationHistoryEntity.LastNegotiatedPrice,
            BestPurchaseOfferPrice = quotationHistoryEntity.BestPurchaseOfferPrice,
            BestSalesOfferPrice = quotationHistoryEntity.BestSalesOfferPrice,
            NumberOfTradesConducted = quotationHistoryEntity.NumberOfTradesConducted,
            TotalQuantityOfTitlesTraded = quotationHistoryEntity.TotalQuantityOfTitlesTraded,
            TotalVolumeOfTitlesNegotiated = quotationHistoryEntity.TotalVolumeOfTitlesNegotiated,
            StrikePriceForOptionsMarket = quotationHistoryEntity.StrikePriceForOptionsMarket,
            CorrectionIndicatorForStrikePrice = quotationHistoryEntity.CorrectionIndicatorForStrikePrice,
            MaturityDateForOptionsMarket = quotationHistoryEntity.MaturityDateForOptionsMarket,
            PaperQuotationFactor = quotationHistoryEntity.PaperQuotationFactor,
            StrikePriceUsdPoints = quotationHistoryEntity.StrikePriceUsdPoints,
            IsinCode = quotationHistoryEntity.IsinCode,
            PaperDistributionNumber = quotationHistoryEntity.PaperDistributionNumber,
        };
    }
}