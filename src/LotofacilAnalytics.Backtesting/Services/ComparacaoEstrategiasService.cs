
using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Services;

public class ComparacaoEstrategiasService
{
    private readonly BacktestingService _backtestingService = new();

    public IReadOnlyCollection<ComparacaoEstrategiaResultado> Comparar(
        IReadOnlyCollection<Concurso> historico,
        IReadOnlyDictionary<string, IEstrategia> estrategias,
        int numeroInicial,
        int numeroFinal)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(estrategias);

        if (estrategias.Count == 0)
        {
            throw new ArgumentException(
                "É necessário informar pelo menos uma estratégia.",
                nameof(estrategias));
        }

        var resultados = new List<ComparacaoEstrategiaResultado>();

        foreach (var (nome, estrategia) in estrategias)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException(
                    "O nome da estratégia não pode ser vazio.",
                    nameof(estrategias));
            }

            ArgumentNullException.ThrowIfNull(estrategia);

            var resultado = _backtestingService.ExecutarConsolidado(
                historico,
                estrategia,
                numeroInicial,
                numeroFinal);

            resultados.Add(
                new ComparacaoEstrategiaResultado(
                    nome,
                    resultado));
        }

        return resultados.AsReadOnly();
    }
}