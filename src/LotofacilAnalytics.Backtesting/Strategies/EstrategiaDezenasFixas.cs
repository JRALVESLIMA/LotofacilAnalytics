
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Strategies;

public class EstrategiaDezenasFixas : IEstrategia
{
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

        var dezenasSelecionadas = Enumerable.Range(1, 15).ToArray();

        IReadOnlyCollection<int> jogo = dezenasSelecionadas;

        return new[] { jogo };
    }
}