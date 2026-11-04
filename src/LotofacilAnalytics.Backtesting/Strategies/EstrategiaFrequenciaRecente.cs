
using LotofacilAnalytics.Analysis.Frequencia;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Strategies;

public class EstrategiaFrequenciaRecente : IEstrategia
{
    private readonly AnaliseFrequenciaRecente _analiseFrequencia = new();
    private readonly int _quantidadeConcursos;

    public EstrategiaFrequenciaRecente(int quantidadeConcursos = 50)
    {
        if (quantidadeConcursos <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeConcursos),
                "A quantidade de concursos deve ser maior que zero.");
        }

        _quantidadeConcursos = quantidadeConcursos;
    }

    public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
        IReadOnlyCollection<Concurso> historico)
    {
        ArgumentNullException.ThrowIfNull(historico);

        if (historico.Count == 0)
        {
            throw new ArgumentException(
                "O histórico não pode estar vazio.",
                nameof(historico));
        }

        var frequencias = _analiseFrequencia.Calcular(
            historico,
            _quantidadeConcursos);

        var dezenasSelecionadas = frequencias
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Key)
            .Take(15)
            .Select(item => item.Key)
            .OrderBy(dezena => dezena)
            .ToArray();

        IReadOnlyCollection<int> jogo = dezenasSelecionadas;

        return new[] { jogo };
    }
}
