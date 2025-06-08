namespace B3.QuotationHistories.Importer.Models;

public class QuotationHistoriesFileImportOptions
{
    public bool DryRun { get; set; } = false;

    public int BatchSize { get; set; } = 100;
}