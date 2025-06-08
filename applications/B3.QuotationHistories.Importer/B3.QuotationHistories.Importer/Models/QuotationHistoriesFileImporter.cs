using System.Globalization;
using B3.QuotationHistories.Importer.Database;
using B3.QuotationHistories.Importer.Database.Models;

namespace B3.QuotationHistories.Importer.Models;

public class QuotationHistoriesFileImporter
{
    private readonly B3QuotationHistoriesDbContext _b3QuotationHistoriesDbContext;

    public QuotationHistoriesFileImporter(
        B3QuotationHistoriesDbContext b3QuotationHistoriesDbContext)
    {
        _b3QuotationHistoriesDbContext = b3QuotationHistoriesDbContext;
    }

    public async Task ImportAsync(string filePath, QuotationHistoriesFileImportOptions options)
    {
        using var fileStreamReader = new StreamReader(filePath);

        string? currentLineContent = null;

        var batch = new List<QuotationHistory>(capacity: options.BatchSize);

        do
        {
            currentLineContent = await fileStreamReader.ReadLineAsync();

            if (currentLineContent is null) continue;

            if (IsHeaderLine(currentLineContent) || IsFooterLine(currentLineContent)) continue;

            var quotationHistory = ParseQuotationHistory(currentLineContent);
            batch.Add(quotationHistory);

            if (batch.Count < options.BatchSize) continue;

            await SaveQuotationHistoriesBatchAsync(batch, options);
            batch.Clear();
        } while (currentLineContent is not null);

        if (batch.Count > 0) await SaveQuotationHistoriesBatchAsync(batch, options);
    }

    private static bool IsHeaderLine(string line)
    {
        return line.StartsWith("00");
    }

    private static bool IsFooterLine(string line)
    {
        return line.StartsWith("99");
    }

    private static QuotationHistory ParseQuotationHistory(string lineContent)
    {
        var bdiCodeRaw = lineContent[10..12];
        var forwardMarketTermInDaysRaw = lineContent[49..52];
        var isinCodeRaw = lineContent[230..242];

        return new QuotationHistory
        {
            NegotiationDate = ParseDate(lineContent[2..10]),
            BdiCode = string.IsNullOrWhiteSpace(bdiCodeRaw) ? null : bdiCodeRaw,
            PaperNegotiationCode = lineContent[12..24].Trim(),
            MarketType = ParseInt(lineContent[24..27]),
            CompanyAbbreviatedName = lineContent[27..39].Trim(),
            PaperSpecification = lineContent[39..49].Trim(),
            ForwardMarketTermInDays = string.IsNullOrWhiteSpace(forwardMarketTermInDaysRaw)
                ? null
                : forwardMarketTermInDaysRaw,
            ReferenceCurrency = lineContent[52..56].Trim(),
            OpeningFloorPrice = ParseDecimal(lineContent[56..69], 2),
            HighestFloorPrice = ParseDecimal(lineContent[69..82], 2),
            LowestFloorPrice = ParseDecimal(lineContent[82..95], 2),
            AverageFloorPrice = ParseDecimal(lineContent[95..108], 2),
            LastNegotiatedPrice = ParseDecimal(lineContent[108..121], 2),
            BestPurchaseOfferPrice = ParseDecimal(lineContent[121..134], 2),
            BestSalesOfferPrice = ParseDecimal(lineContent[134..147], 2),
            NumberOfTradesConducted = ParseLong(lineContent[147..152]),
            TotalQuantityOfTitlesTraded = ParseLong(lineContent[152..170]),
            TotalVolumeOfTitlesNegotiated = ParseDecimal(lineContent[170..188], 2),
            StrikePriceForOptionsMarket = ParseDecimal(lineContent[188..201], 2),
            CorrectionIndicatorForStrikePrice = ParseShort(lineContent[201..202]),
            MaturityDateForOptionsMarket = ParseDate(lineContent[202..210]),
            PaperQuotationFactor = ParseInt(lineContent[210..217]),
            StrikePriceUsdPoints = ParseDecimal(lineContent[217..230], 6),
            IsinCode = string.IsNullOrWhiteSpace(isinCodeRaw) ? null : isinCodeRaw,
            PaperDistributionNumber = ParseInt(lineContent[242..245])
        };
    }

    private static DateOnly? ParseDate(string raw)
    {
        if (raw == "99991231")
            return null;

        return DateOnly.TryParseExact(raw, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;
    }

    private static decimal? ParseDecimal(string raw, int scale)
        => decimal.TryParse(raw.Trim().Insert(raw.Length - scale, "."), NumberStyles.Any, CultureInfo.InvariantCulture,
            out var value)
            ? value
            : null;

    private static int? ParseInt(string raw)
        => int.TryParse(raw.Trim(), out var value) ? value : null;

    private static long? ParseLong(string raw)
        => long.TryParse(raw.Trim(), out var value) ? value : null;

    private static short? ParseShort(string raw)
        => short.TryParse(raw.Trim(), out var value) ? value : null;

    private async Task SaveQuotationHistoriesBatchAsync(IEnumerable<QuotationHistory> batch,
        QuotationHistoriesFileImportOptions options)
    {
        if (!options.DryRun)
        {
            await _b3QuotationHistoriesDbContext.QuotationHistories.AddRangeAsync(batch);
            await _b3QuotationHistoriesDbContext.SaveChangesAsync();
            _b3QuotationHistoriesDbContext.ChangeTracker.Clear();
        }
    }
}