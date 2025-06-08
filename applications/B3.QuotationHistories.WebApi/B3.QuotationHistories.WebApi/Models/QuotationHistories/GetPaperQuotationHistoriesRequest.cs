using System.ComponentModel.DataAnnotations;
using B3.QuotationHistories.Application.Common;

namespace B3.QuotationHistories.WebApi.Models.QuotationHistories;

public class GetPaperQuotationHistoriesRequest
{
    [Required(ErrorMessage = "The {0} parameter is required.")]
    public required string PaperNegotiationCode { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = PaginationSettings.DefaultPageSize;
}