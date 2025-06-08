using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B3.QuotationHistories.Infrastructure.Persistence.Models;

[Table("quotation_histories")]
public class QuotationHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("negotiation_date")]
    public DateOnly? NegotiationDate { get; set; }

    [Column("bdi_code")]
    public string? BdiCode { get; set; }

    [Column("paper_negotiation_code")]
    public string? PaperNegotiationCode { get; set; }

    [Column("market_type")]
    public int? MarketType { get; set; }

    [Column("company_abbreviated_name")]
    public string? CompanyAbbreviatedName { get; set; }

    [Column("paper_specification")]
    public string? PaperSpecification { get; set; }

    [Column("forward_market_term_in_days")]
    public string? ForwardMarketTermInDays { get; set; }

    [Column("reference_currency")]
    public string? ReferenceCurrency { get; set; }

    [Column("opening_floor_price")]
    public decimal? OpeningFloorPrice { get; set; }

    [Column("highest_floor_price")]
    public decimal? HighestFloorPrice { get; set; }

    [Column("lowest_floor_price")]
    public decimal? LowestFloorPrice { get; set; }

    [Column("average_floor_price")]
    public decimal? AverageFloorPrice { get; set; }

    [Column("last_negotiated_price")]
    public decimal? LastNegotiatedPrice { get; set; }

    [Column("best_purchase_offer_price")]
    public decimal? BestPurchaseOfferPrice { get; set; }

    [Column("best_sales_offer_price")]
    public decimal? BestSalesOfferPrice { get; set; }

    [Column("number_of_trades_conducted")]
    public long? NumberOfTradesConducted { get; set; }

    [Column("total_quantity_of_titles_traded")]
    public long? TotalQuantityOfTitlesTraded { get; set; }

    [Column("total_volume_of_titles_negotiated")]
    public decimal? TotalVolumeOfTitlesNegotiated { get; set; }

    [Column("strike_price_for_options_market")]
    public decimal? StrikePriceForOptionsMarket { get; set; }

    [Column("correction_indicator_for_strike_price")]
    public short? CorrectionIndicatorForStrikePrice { get; set; }

    [Column("maturity_date_for_options_market")]
    public DateOnly? MaturityDateForOptionsMarket { get; set; }

    [Column("paper_quotation_factor")]
    public int? PaperQuotationFactor { get; set; }

    [Column("strike_price_usd_points")]
    public decimal? StrikePriceUsdPoints { get; set; }

    [Column("isin_code")]
    public string? IsinCode { get; set; }

    [Column("paper_distribution_number")]
    public int? PaperDistributionNumber { get; set; }
}