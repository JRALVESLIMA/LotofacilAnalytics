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

    public Task<int?> ObterUltimoConcursoSalvoAsync()
    {
        var arquivos = Directory.GetFiles(
            _diretorio,
            "concurso_*.json");

        if (arquivos.Length == 0)
        {
            return Task.FromResult<int?>(null);
        }

        var numeros = arquivos
            .Select(Path.GetFileNameWithoutExtension)
            .Select(nome => nome!.Replace("concurso_", ""))
            .Select(numero => int.TryParse(numero, out var resultado)
                ? resultado
                : (int?)null)
            .Where(numero => numero.HasValue)
            .Select(numero => numero!.Value);

        var ultimoConcurso = numeros.Any()
            ? numeros.Max()
            : (int?)null;

        return Task.FromResult(ultimoConcurso);

        
    }
}

