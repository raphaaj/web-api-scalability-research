namespace B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;

public class TopAssetWithHighestNegotiatedVolumeDto(string paperNegotiationCode, decimal totalVolumeOfTilesNegotiated)
{
    public string PaperNegotiationCode { get; set; } = paperNegotiationCode;
    public decimal TotalVolumeOfTilesNegotiated { get; set; } = totalVolumeOfTilesNegotiated;
}