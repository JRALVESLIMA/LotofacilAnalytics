using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Backtesting.Strategies;

namespace LotofacilAnalytics.Backtesting.Services;

public class BacktestingService
{
    public List<Concurso> ObterHistoricoDisponivel(
        IEnumerable<Concurso> historico,
        Concurso concursoAlvo)
    {
        return historico
            .Where(concurso => concurso.Numero < concursoAlvo.Numero)
            .ToList();
    }

    public RodadaBacktesting PrepararRodada(
        IEnumerable<Concurso> historico,
        Concurso concursoAlvo,
        IEstrategia estrategia)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(concursoAlvo);
        ArgumentNullException.ThrowIfNull(estrategia);

        var historicoDisponivel = ObterHistoricoDisponivel(
            historico,
            concursoAlvo);

        var jogosGerados = estrategia.GerarJogos(historicoDisponivel);

        var avaliador = new AvaliadorJogo();

        var resultadosJogos = jogosGerados
            .Select(jogo =>
            {
                var acertos = avaliador.CalcularAcertos(
                    jogo,
                    concursoAlvo.Dezenas);

                return new ResultadoJogo(
                    jogo,
                    acertos);
            })
            .ToList();

        return new RodadaBacktesting(
            concursoAlvo,
            historicoDisponivel,
            resultadosJogos);
    }

    public List<RodadaBacktesting> Executar(
        IEnumerable<Concurso> historico,
        IEstrategia estrategia)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(estrategia);

        var concursos = historico
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        return concursos
            .Skip(1)
            .Select(concurso => PrepararRodada(
                concursos,
                concurso,
                estrategia))
            .ToList();
    }
}