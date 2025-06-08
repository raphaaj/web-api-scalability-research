namespace B3.QuotationHistories.WebApi.Models.Assets;

public class GetTopNAssetsWithHighestNegotiatedVolumeResponse(TopAssetWithHighestNegotiatedVolumeResponse[] assets)
{
    public TopAssetWithHighestNegotiatedVolumeResponse[] Assets { get; set; } = assets;
}

public class TopAssetWithHighestNegotiatedVolumeResponse(
    string paperNegotiationCode,
    decimal totalVolumeOfTilesNegotiated)
{
    public string PaperNegotiationCode { get; set; } = paperNegotiationCode;
    public decimal TotalVolumeOfTilesNegotiated { get; set; } = totalVolumeOfTilesNegotiated;
}