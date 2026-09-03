using System.Text.Json;
using LotofacilAnalytics.DataCollector.Models;

namespace LotofacilAnalytics.DataCollector.Storage;

public class ConcursoJsonStorage
{
    private readonly string _diretorio;

    public ConcursoJsonStorage(string diretorio)
    {

        _diretorio = diretorio;

        Directory.CreateDirectory(_diretorio);
    }

    public async Task SalvarAsync(ConcursoCaixa concurso)
    {
        var nomeArquivo =  $"concurso_{concurso.Numero:D4}.json";

        var caminhoArquivo = Path.Combine(_diretorio, nomeArquivo);

        var json = JsonSerializer.Serialize(
            concurso,
            new JsonSerializerOptions
            {
                WriteIndented = true
            }
        );

        await File.WriteAllTextAsync(caminhoArquivo, json);
    }
}