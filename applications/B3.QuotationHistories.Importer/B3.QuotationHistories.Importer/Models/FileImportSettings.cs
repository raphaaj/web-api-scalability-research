namespace B3.QuotationHistories.Importer.Models;

public class FileImportSettings
{
    public string? FilePath { get; set; }

    public bool? DryRun { get; set; }

    public int? BatchSize { get; set; }
}