
using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Services;

public class ComparacaoPeriodosService
{
    private readonly BacktestingService _backtestingService = new();

    public IReadOnlyCollection<ComparacaoPeriodoResultado> Comparar(
        IEnumerable<Concurso> historico,
        IEnumerable<(int Inicio, int Fim)> periodos,
        IEnumerable<(string Nome, IEstrategia Estrategia)> estrategias)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(periodos);
        ArgumentNullException.ThrowIfNull(estrategias);

        var concursos = historico
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        var listaPeriodos = periodos.ToList();
        var listaEstrategias = estrategias.ToList();

        var resultados = new List<ComparacaoPeriodoResultado>();

        foreach (var periodo in listaPeriodos)
        {
            if (periodo.Inicio <= 0 || periodo.Fim < periodo.Inicio)
            {
                throw new ArgumentException(
                    "O período informado é inválido.",
                    nameof(periodos));
            }

            foreach (var estrategia in listaEstrategias)
            {
                var resultado = _backtestingService.ExecutarConsolidado(
                    concursos,
                    estrategia.Estrategia,
                    periodo.Inicio,
                    periodo.Fim);

                resultados.Add(new ComparacaoPeriodoResultado(
                    periodo.Inicio,
                    periodo.Fim,
                    estrategia.Nome,
                    resultado));
            }
        }

        return resultados;
    }
}