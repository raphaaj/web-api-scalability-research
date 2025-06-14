using B3.QuotationHistories.Domain.Entities;
using B3.QuotationHistories.Domain.ValueObjects;
using B3.QuotationHistories.Infrastructure.Persistence.Models;

namespace B3.QuotationHistories.Infrastructure.Persistence.Mappers;

public static class QuotationHistoryModelMapper
{
    public static QuotationHistoryEntity ToQuotationHistoryEntity(QuotationHistory quotationHistory)
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