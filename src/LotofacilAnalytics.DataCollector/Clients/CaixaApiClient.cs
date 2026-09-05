using System.Text.Json;
using LotofacilAnalytics.DataCollector.Models;

namespace LotofacilAnalytics.DataCollector.Clients;

public class CaixaApiClient
{
    private readonly HttpClient _httpClient;

    public CaixaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ConcursoCaixa?> ObterUltimoConcursoAsync()
    {
        const string url =
            "https://servicebus2.caixa.gov.br/portaldeloterias/api/lotofacil";

        return await ObterAsync(url);
    }

    public async Task<ConcursoCaixa?> ObterConcursoAsync(int numeroConcurso)
    {
        var url =
            $"https://servicebus2.caixa.gov.br/portaldeloterias/api/lotofacil/{numeroConcurso}";

        return await ObterAsync(url);
    }

    private async Task<ConcursoCaixa?> ObterAsync(string url)
    {
        const int maxTentativas = 3;

        for (var tentativa = 1; tentativa <= maxTentativas; tentativa++)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json =
                    await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<ConcursoCaixa>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (HttpRequestException ex)
            {
                if (tentativa == maxTentativas)
                {
                    throw;
                }

                Console.WriteLine(
                    $"    ⚠ Falha na tentativa {tentativa}: {ex.Message}");

                Console.WriteLine(
                    $"    Aguardando antes de tentar novamente...");

                await Task.Delay(1000 * tentativa);
            }
        }

        return null;
    }
}