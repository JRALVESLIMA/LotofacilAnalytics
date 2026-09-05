using LotofacilAnalytics.DataCollector.Clients;
using LotofacilAnalytics.DataCollector.Services;
using LotofacilAnalytics.DataCollector.Storage;
using LotofacilAnalytics.DataCollector.Validation;

using var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30)
};

var caixaApiClient = new CaixaApiClient(httpClient);

var diretorioRaw = Path.Combine(
    Directory.GetCurrentDirectory(),
    "data",
    "raw");

var storage = new ConcursoJsonStorage(diretorioRaw);

var validator = new ConcursoValidator();

var updateService = new ConcursoUpdateService(
    caixaApiClient,
    storage,
    validator);

await updateService.AtualizarAsync(3);