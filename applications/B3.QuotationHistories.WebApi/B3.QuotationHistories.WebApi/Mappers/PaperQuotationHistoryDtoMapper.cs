using B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;
using B3.QuotationHistories.WebApi.Models.QuotationHistories;

namespace B3.QuotationHistories.WebApi.Mappers;

public static class PaperQuotationHistoryDtoMapper
{
    public static GetPaperQuotationHistoryResponse ToGetPaperHistoricalQuotationResponse(
        PaperQuotationHistoryDto paperQuotationHistory)
    {
        return new GetPaperQuotationHistoryResponse
        {
            Id = paperQuotationHistory.Id,
            NegotiationDate = paperQuotationHistory.NegotiationDate,
            BdiCode = paperQuotationHistory.BdiCode,
            PaperNegotiationCode = paperQuotationHistory.PaperNegotiationCode,
            MarketType = paperQuotationHistory.MarketType,
            CompanyAbbreviatedName = paperQuotationHistory.CompanyAbbreviatedName,
            PaperSpecification = paperQuotationHistory.PaperSpecification,
            ForwardMarketTermInDays = paperQuotationHistory.ForwardMarketTermInDays,
            ReferenceCurrency = paperQuotationHistory.ReferenceCurrency,
            OpeningFloorPrice = paperQuotationHistory.OpeningFloorPrice,
            HighestFloorPrice = paperQuotationHistory.HighestFloorPrice,
            LowestFloorPrice = paperQuotationHistory.LowestFloorPrice,
            AverageFloorPrice = paperQuotationHistory.AverageFloorPrice,
            LastNegotiatedPrice = paperQuotationHistory.LastNegotiatedPrice,
            BestPurchaseOfferPrice = paperQuotationHistory.BestPurchaseOfferPrice,
            BestSalesOfferPrice = paperQuotationHistory.BestSalesOfferPrice,
            NumberOfTradesConducted = paperQuotationHistory.NumberOfTradesConducted,
            TotalQuantityOfTitlesTraded = paperQuotationHistory.TotalQuantityOfTitlesTraded,
            TotalVolumeOfTitlesNegotiated = paperQuotationHistory.TotalVolumeOfTitlesNegotiated,
            StrikePriceForOptionsMarket = paperQuotationHistory.StrikePriceForOptionsMarket,
            CorrectionIndicatorForStrikePrice = paperQuotationHistory.CorrectionIndicatorForStrikePrice,
            MaturityDateForOptionsMarket = paperQuotationHistory.MaturityDateForOptionsMarket,
            PaperQuotationFactor = paperQuotationHistory.PaperQuotationFactor,
            StrikePriceUsdPoints = paperQuotationHistory.StrikePriceUsdPoints,
            IsinCode = paperQuotationHistory.IsinCode,
            PaperDistributionNumber = paperQuotationHistory.PaperDistributionNumber,
        };
    }
}