using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Strategies;

public interface IEstrategia
{
    IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
        IReadOnlyCollection<Concurso> historico);
}