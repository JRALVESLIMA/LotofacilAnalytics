
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
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(concursoAlvo);

        return historico
            .Where(concurso => concurso.Numero < concursoAlvo.Numero)
            .OrderBy(concurso => concurso.Numero)
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

        if (jogosGerados is null || jogosGerados.Count == 0)
        {
            throw new InvalidOperationException(
                "A estratégia não gerou nenhum jogo para avaliar.");
        }

        var avaliador = new AvaliadorJogo();

        var resultadosJogos = jogosGerados
            .Select(jogo =>
            {
                var acertos = avaliador.CalcularAcertos(
                    jogo,
                    concursoAlvo.Dezenas);

                return new ResultadoJogo(jogo, acertos);
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

        if (concursos.Count <= 1)
        {
            return new List<RodadaBacktesting>();
        }

        return Executar(
            concursos,
            estrategia,
            concursos[1].Numero,
            concursos[^1].Numero);
    }

    public List<RodadaBacktesting> Executar(
        IEnumerable<Concurso> historico,
        IEstrategia estrategia,
        int numeroInicial,
        int numeroFinal)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(estrategia);

        if (numeroInicial <= 0 || numeroFinal < numeroInicial)
        {
            throw new ArgumentException(
                "O período informado é inválido.");
        }

        var concursos = historico
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        if (concursos.Count <= 1)
        {
            return new List<RodadaBacktesting>();
        }

        var primeiroNumero = concursos[0].Numero;

        var concursosAlvo = concursos
            .Where(concurso =>
                concurso.Numero >= numeroInicial &&
                concurso.Numero <= numeroFinal &&
                concurso.Numero > primeiroNumero)
            .ToList();

        return concursosAlvo
            .Select(concurso => PrepararRodada(
                concursos,
                concurso,
                estrategia))
            .ToList();
    }

    public BacktestingResultado ExecutarConsolidado(
        IEnumerable<Concurso> historico,
        IEstrategia estrategia)
    {
        var rodadas = Executar(historico, estrategia);

        return new BacktestingResultado(rodadas);
    }

    public BacktestingResultado ExecutarConsolidado(
        IEnumerable<Concurso> historico,
        IEstrategia estrategia,
        int numeroInicial,
        int numeroFinal)
    {
        var rodadas = Executar(
            historico,
            estrategia,
            numeroInicial,
            numeroFinal);

        return new BacktestingResultado(rodadas);
    }
}