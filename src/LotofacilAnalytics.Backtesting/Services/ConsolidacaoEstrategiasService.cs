
using LotofacilAnalytics.Backtesting.Models;

namespace LotofacilAnalytics.Backtesting.Services;

public class ConsolidacaoEstrategiasService
{
    public IReadOnlyCollection<ConsolidacaoEstrategiaResultado>
        Consolidar(
            IEnumerable<ComparacaoPeriodoResultado> resultados)
    {
        ArgumentNullException.ThrowIfNull(resultados);

        var grupos = resultados
            .GroupBy(resultado => resultado.NomeEstrategia);

        var consolidados =
            new List<ConsolidacaoEstrategiaResultado>();

        foreach (var grupo in grupos)
        {
            var periodosOrdenados = grupo
                .OrderBy(resultado => resultado.NumeroInicial)
                .ToArray();

            for (int i = 1; i < periodosOrdenados.Length; i++)
            {
                var anterior = periodosOrdenados[i - 1];
                var atual = periodosOrdenados[i];

                if (atual.NumeroInicial <= anterior.NumeroFinal)
                {
                    throw new ArgumentException(
                        $"Períodos sobrepostos para a estratégia " +
                        $"'{grupo.Key}': " +
                        $"{anterior.NumeroInicial}-{anterior.NumeroFinal} e " +
                        $"{atual.NumeroInicial}-{atual.NumeroFinal}.");
                }
            }

            var rodadas = periodosOrdenados
                .SelectMany(resultado => resultado.Resultado.Rodadas)
                .ToArray();
            
            var concursosDuplicados = rodadas
                .GroupBy(rodada => rodada.ConcursoAlvo.Numero)
                .Where(grupo => grupo.Count() > 1)
                .Select(grupo => grupo.Key)
                .OrderBy(numero => numero)
                .ToArray();

            if (concursosDuplicados.Length > 0)
            {
                throw new ArgumentException(
                    $"A estratégia '{grupo.Key}' possui concursos " +
                    $"duplicados na consolidação: " +
                    $"{string.Join(", ", concursosDuplicados)}.");
            }

            var resultadoConsolidado =
                new BacktestingResultado(rodadas);

            var consolidacao =
                new ConsolidacaoEstrategiaResultado(
                    grupo.Key,
                    periodosOrdenados.Length,
                    resultadoConsolidado);

            consolidados.Add(consolidacao);
        }

        return consolidados;
    }
}