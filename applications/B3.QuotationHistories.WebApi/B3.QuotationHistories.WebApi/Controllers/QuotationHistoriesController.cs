using B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;
using B3.QuotationHistories.WebApi.Mappers;
using B3.QuotationHistories.WebApi.Models.QuotationHistories;
using Microsoft.AspNetCore.Mvc;

namespace B3.QuotationHistories.WebApi.Controllers;

[ApiController]
[Route("api/quotation-histories")]
public class QuotationHistoriesController(GetPaperQuotationHistoriesUseCase getPaperQuotationHistoriesUseCase)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPaperQuotationHistoriesAsync(
        [FromQuery] GetPaperQuotationHistoriesRequest request)
    {
        var getPaperHistoricalQuotationsCommandRequest = new GetPaperQuotationHistoriesUseCaseCommandRequest
        {
            PaperNegotiationCode = request.PaperNegotiationCode,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };

        var getPaperHistoricalQuotationsCommandResult =
            await getPaperQuotationHistoriesUseCase.ExecuteAsync(getPaperHistoricalQuotationsCommandRequest);

        var getPaperHistoricalQuotationsItemsResponse = getPaperHistoricalQuotationsCommandResult.Items
            .Select(PaperQuotationHistoryDtoMapper.ToGetPaperHistoricalQuotationResponse)
            .ToArray();

        var getPaperHistoricalQuotationsResponse = new GetPaperQuotationHistoriesResponse(
            getPaperHistoricalQuotationsCommandResult.PageNumber,
            getPaperHistoricalQuotationsCommandResult.PageSize,
            getPaperHistoricalQuotationsCommandResult.TotalNumberOfItems,
            getPaperHistoricalQuotationsCommandResult.TotalNumberOfPages,
            getPaperHistoricalQuotationsItemsResponse);

        return Ok(getPaperHistoricalQuotationsResponse);
    }
}