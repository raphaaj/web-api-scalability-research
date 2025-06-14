using System.ComponentModel.DataAnnotations;
using B3.QuotationHistories.Application.Exceptions;
using B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;
using B3.QuotationHistories.WebApi.Mappers;
using B3.QuotationHistories.WebApi.Models.Assets;
using Microsoft.AspNetCore.Mvc;

namespace B3.QuotationHistories.WebApi.Controllers;

[ApiController]
[Route("api/assets")]
public class AssetsController(
    GetTopNAssetsWithHighestNegotiatedVolumeUseCase getTopNAssetsWithHighestNegotiatedVolumeUseCase)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTopNAssetsWithHighestNegotiatedVolumeAsync(
        [FromQuery,
         Range(GetTopNAssetsWithHighestNegotiatedVolumeUseCase.MinNumberOfTopAssetsWithHighestNegotiatedVolume,
             GetTopNAssetsWithHighestNegotiatedVolumeUseCase.MaxNumberOfTopAssetsWithHighestNegotiatedVolume)]
        int n)
    {
        var getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest =
            new GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest
            {
                NumberOfTopAssetsWithHighestNegotiatedVolume = n
            };

        GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult
            getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult;
        try
        {
            getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult =
                await getTopNAssetsWithHighestNegotiatedVolumeUseCase.ExecuteAsync(
                    getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest);
        }
        catch (InvalidNumberOfTopAssetsWithHighestNegotiatedVolumeException
               invalidNumberOfTopAssetsWithHighestNegotiatedVolumeException)
        {
            ModelState.AddModelError(nameof(n), invalidNumberOfTopAssetsWithHighestNegotiatedVolumeException.Message);
            return ValidationProblem(ModelState);
        }

        var assets = getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult
            .Assets
            .Select(TopAssetWithHighestNegotiatedVolumeDtoMapper.ToTopAssetWithHighestNegotiatedVolumeResponse)
            .ToArray();

        var getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResponse =
            new GetTopNAssetsWithHighestNegotiatedVolumeResponse(assets);

        return Ok(getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResponse);
    }
}