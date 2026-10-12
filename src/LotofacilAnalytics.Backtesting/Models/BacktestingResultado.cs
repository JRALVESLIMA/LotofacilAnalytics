
namespace LotofacilAnalytics.Backtesting.Models;

public class BacktestingResultado
{
    public IReadOnlyCollection<RodadaBacktesting> Rodadas { get; }

    public int QuantidadeRodadas => Rodadas.Count;
    
    public int QuantidadeJogos =>
        Rodadas.Sum(rodada => rodada.ResultadosJogos.Count);

    public double MediaAcertos =>
        QuantidadeJogos == 0
            ? 0
            : Rodadas
                .SelectMany(rodada => rodada.ResultadosJogos)
                .Average(resultado => resultado.QuantidadeAcertos);

    public int MaiorAcertos =>
        Rodadas
            .SelectMany(rodada => rodada.ResultadosJogos)
            .Select(resultado => resultado.QuantidadeAcertos)
            .DefaultIfEmpty(0)
            .Max();

    public int MenorAcertos =>
        Rodadas
            .SelectMany(rodada => rodada.ResultadosJogos)
            .Select(resultado => resultado.QuantidadeAcertos)
            .DefaultIfEmpty(0)
            .Min();

    public int QuantidadeJogosComAcertos(int quantidadeAcertos) =>
        Rodadas
            .SelectMany(rodada => rodada.ResultadosJogos)
            .Count(resultado => resultado.QuantidadeAcertos == quantidadeAcertos);

    public IReadOnlyDictionary<int, int> ObterDistribuicaoAcertos()
    {
        return Rodadas
            .SelectMany(rodada => rodada.ResultadosJogos)
            .GroupBy(resultado => resultado.QuantidadeAcertos)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Count());
    }

    public BacktestingResultado(
        IEnumerable<RodadaBacktesting> rodadas)
    {
        ArgumentNullException.ThrowIfNull(rodadas);

        Rodadas = rodadas.ToArray();
    }
}