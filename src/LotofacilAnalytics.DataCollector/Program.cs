using LotofacilAnalytics.DataCollector.Clients;
using LotofacilAnalytics.DataCollector.Validation;

using var httpClient = new HttpClient();

var caixaApiClient = new CaixaApiClient(httpClient);

var concurso = await caixaApiClient.ObterConcursoAsync(1);

if (concurso is null)
{
    Console.WriteLine("Não foi possível obter o concurso.");
    return;
}

var validator = new ConcursoValidator();

var valido = validator.Validar(concurso);

Console.WriteLine($"Concurso: {concurso.Numero}");
Console.WriteLine($"Data: {concurso.DataApuracao}");
Console.WriteLine($"Quantidade de dezenas: {concurso.ListaDezenas.Count}");
Console.WriteLine($"Concurso válido: {valido}");
