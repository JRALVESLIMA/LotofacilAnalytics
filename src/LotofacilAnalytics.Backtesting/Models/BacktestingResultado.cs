
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

    public double VarianciaAcertos
    {
        get
        {
            if (QuantidadeJogos == 0)
                return 0;

            var media = MediaAcertos;

            return Rodadas
                .SelectMany(rodada => rodada.ResultadosJogos)
                .Average(resultado =>
                    Math.Pow(resultado.QuantidadeAcertos - media, 2));
        }
    }

    public double DesvioPadraoAcertos =>
        Math.Sqrt(VarianciaAcertos);

    public double MedianaAcertos
    {
        get
        {
            var acertos = Rodadas
                .SelectMany(rodada => rodada.ResultadosJogos)
                .Select(resultado => resultado.QuantidadeAcertos)
                .OrderBy(valor => valor)
                .ToArray();

            if (acertos.Length == 0)
                return 0;

            var meio = acertos.Length / 2;

            if (acertos.Length % 2 != 0)
                return acertos[meio];

            return (acertos[meio - 1] + acertos[meio]) / 2.0;
        }
    }

    public double PercentualJogosCom11OuMaisAcertos
    {
        get
        {
            if (QuantidadeJogos == 0)
                return 0;

            var jogosCom11OuMais = Rodadas
                .SelectMany(rodada => rodada.ResultadosJogos)
                .Count(resultado => resultado.QuantidadeAcertos >= 11);

            return jogosCom11OuMais * 100.0 / QuantidadeJogos;
        }
    }

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