using System.ComponentModel.DataAnnotations.Schema;

namespace B3.QuotationHistories.Infrastructure.Persistence.Models;

public class AssetAggregation
{
    [Column("paper_negotiation_code")]
    public required string PaperNegotiationCode { get; set; }

    [Column("total_volume_of_titles_negotiated")]
    public decimal TotalVolumeOfTitlesNegotiated { get; set; }
}