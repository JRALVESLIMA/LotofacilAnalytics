using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Models;

public class RodadaBacktesting
{
    public Concurso ConcursoAlvo { get; }

    public IReadOnlyCollection<Concurso> HistoricoDisponivel { get; }

    public IReadOnlyCollection<ResultadoJogo> ResultadosJogos { get; }

    public RodadaBacktesting(
        Concurso concursoAlvo,
        IReadOnlyCollection<Concurso> historicoDisponivel,
        IReadOnlyCollection<ResultadoJogo> resultadosJogos)
    {
        ArgumentNullException.ThrowIfNull(concursoAlvo);
        ArgumentNullException.ThrowIfNull(historicoDisponivel);
        ArgumentNullException.ThrowIfNull(resultadosJogos);

        ConcursoAlvo = concursoAlvo;
        HistoricoDisponivel = historicoDisponivel.ToArray();
        ResultadosJogos = resultadosJogos.ToArray();
    }
}