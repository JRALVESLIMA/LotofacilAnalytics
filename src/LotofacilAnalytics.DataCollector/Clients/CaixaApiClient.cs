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

    public async Task<ConcursoCaixa?> ObterUltimoConccursoAsync()
    {
        const string url = "https://servicebus2.caixa.gov.br/portaldeloterias/api/lotofacil";

        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ConcursoCaixa>(
            json, 
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            }
        );
    }


    public async Task<ConcursoCaixa?> ObterConcursoAsync(int numeroConcurso)
    {
        var url = $"https://servicebus2.caixa.gov.br/portaldeloterias/api/lotofacil/{numeroConcurso}";

        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ConcursoCaixa>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );
    }
}