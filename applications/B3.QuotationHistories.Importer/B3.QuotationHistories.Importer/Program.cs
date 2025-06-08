using B3.QuotationHistories.Importer.Database;
using B3.QuotationHistories.Importer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var b3HistoricalQuotations2024ConnectionString = config.GetConnectionString("B3HistoricalQuotations2024Connection");

var b3HistoricalQuotationsDbContextOptionsBuilder = new DbContextOptionsBuilder<B3QuotationHistoriesDbContext>();
b3HistoricalQuotationsDbContextOptionsBuilder
    .UseNpgsql(b3HistoricalQuotations2024ConnectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Warning);

var fileImportSettings = new FileImportSettings();
config.GetSection("FileImport").Bind(fileImportSettings);

if (fileImportSettings.FilePath is null)
    throw new Exception("É necessário informar o caminho do arquivo via configuração FileImport:FilePath");

if (!File.Exists(fileImportSettings.FilePath))
    throw new Exception($"Arquivo {fileImportSettings.FilePath} não localizado");

await using var db = new B3QuotationHistoriesDbContext(b3HistoricalQuotationsDbContextOptionsBuilder.Options);

var quotationFileHistoryImporter = new QuotationHistoriesFileImporter(db);
var quotationFileHistoryImportOptions = new QuotationHistoriesFileImportOptions
{
    DryRun = fileImportSettings.DryRun ?? false,
    BatchSize = fileImportSettings.BatchSize ?? 100
};

Console.WriteLine($"Iniciando importação de arquivo de cotações históricas: {fileImportSettings.FilePath}");

try
{
    await quotationFileHistoryImporter.ImportAsync(fileImportSettings.FilePath, quotationFileHistoryImportOptions);

    Console.WriteLine("Importação de arquivo de cotações históricas concluída!");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro inesperado ao processar importação: {ex.Message}");
    Console.WriteLine(ex.ToString());
}