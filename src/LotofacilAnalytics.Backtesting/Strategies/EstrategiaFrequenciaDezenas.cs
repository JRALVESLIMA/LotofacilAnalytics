
using LotofacilAnalytics.Analysis.Frequencia;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Strategies;

public class EstrategiaFrequenciaDezenas : IEstrategia
{
    private readonly AnaliseFrequenciaDezenas _analiseFrequencia;

    public EstrategiaFrequenciaDezenas()
    {
        _analiseFrequencia = new AnaliseFrequenciaDezenas();
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

        var frequencias = _analiseFrequencia.CalcularFrequencia(historico);

        var dezenasSelecionadas = frequencias
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Key)
            .Take(15)
            .Select(item => item.Key)
            .OrderBy(dezena => dezena)
            .ToArray();

        IReadOnlyCollection<int> jogo = dezenasSelecionadas;

        return new IReadOnlyCollection<int>[] { jogo };
    }
}