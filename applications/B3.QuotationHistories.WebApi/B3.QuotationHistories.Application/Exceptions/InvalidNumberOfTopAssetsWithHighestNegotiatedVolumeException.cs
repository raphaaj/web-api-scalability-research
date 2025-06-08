namespace B3.QuotationHistories.Application.Exceptions;

public class InvalidNumberOfTopAssetsWithHighestNegotiatedVolumeException(
    string message,
    int minNumberOfTopAssetsWithHighestNegotiatedVolume,
    int maxNumberOfTopAssetsWithHighestNegotiatedVolume)
    : Exception(message)
{
    public int MinNumberOfTopAssetsWithHighestNegotiatedVolume { get; set; } =
        minNumberOfTopAssetsWithHighestNegotiatedVolume;

    public int MaxNumberOfTopAssetsWithHighestNegotiatedVolume { get; set; } =
        maxNumberOfTopAssetsWithHighestNegotiatedVolume;
}