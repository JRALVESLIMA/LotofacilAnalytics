using System.Text.Json;
using LotofacilAnalytics.DataCollector.Mappers;
using LotofacilAnalytics.DataCollector.Models;
using LotofacilAnalytics.DataCollector.Validation;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.DataCollector.Storage;

public class ConcursoHistoricoReader
{
    private readonly string _diretorio;
    private readonly ConcursoValidator _validator;
    private readonly ConcursoMapper _mapper;

    public ConcursoHistoricoReader(
        string diretorio,
        ConcursoValidator validator,
        ConcursoMapper mapper)
    {
        _diretorio = diretorio;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<List<Concurso>> LerTodosAsync()
    {
        var arquivos = Directory
            .GetFiles(_diretorio, "concurso_*.json")
            .OrderBy(arquivo => arquivo)
            .ToList();

        var concursos = new List<Concurso>();

        foreach (var arquivo in arquivos)
        {
            var json = await File.ReadAllTextAsync(arquivo);

            var concursoCaixa =
                JsonSerializer.Deserialize<ConcursoCaixa>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (concursoCaixa is null)
            {
                throw new InvalidOperationException(
                    $"Não foi possível ler o arquivo: {arquivo}");
            }

            if (!_validator.Validar(concursoCaixa))
            {
                throw new InvalidOperationException(
                    $"Concurso inválido no arquivo: {arquivo}");
            }

            var concurso = _mapper.Mapear(concursoCaixa);

            concursos.Add(concurso);
        }

        return concursos;
    }
}