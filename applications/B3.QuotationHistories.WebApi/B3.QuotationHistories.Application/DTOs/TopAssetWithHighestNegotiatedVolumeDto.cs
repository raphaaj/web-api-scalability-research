using B3.QuotationHistories.Domain.ValueObjects;

namespace B3.QuotationHistories.Application.DTOs;

public class TopAssetWithHighestNegotiatedVolumeDto(string paperNegotiationCode, decimal totalVolumeOfTilesNegotiated)
{
    public PaperNegotiationCode PaperNegotiationCode { get; set; } = PaperNegotiationCode.Create(paperNegotiationCode);

    public decimal TotalVolumeOfTilesNegotiated { get; set; } = totalVolumeOfTilesNegotiated;
}