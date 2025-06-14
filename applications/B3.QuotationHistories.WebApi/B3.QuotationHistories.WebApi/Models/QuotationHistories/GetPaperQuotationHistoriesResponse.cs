namespace B3.QuotationHistories.WebApi.Models.QuotationHistories;

public class GetPaperQuotationHistoriesResponse(GetPaperQuotationHistoryResponse[] quotationHistories)
{
    public GetPaperQuotationHistoryResponse[] QuotationHistories { get; set; } = quotationHistories;
}

public class GetPaperQuotationHistoryResponse
{
    public int Id { get; set; }

    public DateOnly? NegotiationDate { get; set; }

    public string? BdiCode { get; set; }

    public string? PaperNegotiationCode { get; set; }

    public int? MarketType { get; set; }

    public string? CompanyAbbreviatedName { get; set; }

    public string? PaperSpecification { get; set; }

    public string? ForwardMarketTermInDays { get; set; }

    public string? ReferenceCurrency { get; set; }

    public decimal? OpeningFloorPrice { get; set; }

    public decimal? HighestFloorPrice { get; set; }

    public decimal? LowestFloorPrice { get; set; }

    public decimal? AverageFloorPrice { get; set; }

    public decimal? LastNegotiatedPrice { get; set; }

    public decimal? BestPurchaseOfferPrice { get; set; }

    public decimal? BestSalesOfferPrice { get; set; }

    public long? NumberOfTradesConducted { get; set; }

    public long? TotalQuantityOfTitlesTraded { get; set; }

    public decimal? TotalVolumeOfTitlesNegotiated { get; set; }

    public decimal? StrikePriceForOptionsMarket { get; set; }

    public short? CorrectionIndicatorForStrikePrice { get; set; }

    public DateOnly? MaturityDateForOptionsMarket { get; set; }

    public int? PaperQuotationFactor { get; set; }

    public decimal? StrikePriceUsdPoints { get; set; }

    public string? IsinCode { get; set; }

    public int? PaperDistributionNumber { get; set; }
}