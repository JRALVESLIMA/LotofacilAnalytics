
namespace LotofacilAnalytics.Backtesting.Models;

public class ComparacaoEstrategiaResultado
{
    public string NomeEstrategia { get; }

    public BacktestingResultado Resultado { get; }

    public int QuantidadeRodadas =>
        Resultado.QuantidadeRodadas;

    public int QuantidadeJogos =>
        Resultado.QuantidadeJogos;

    public double MediaAcertos =>
        Resultado.MediaAcertos;

    public int MaiorAcertos =>
        Resultado.MaiorAcertos;

    public int MenorAcertos =>
        Resultado.MenorAcertos;

    public int JogosCom11OuMaisAcertos =>
        ContarJogosComMinimoDeAcertos(11);

    public int JogosCom13OuMaisAcertos =>
        ContarJogosComMinimoDeAcertos(13);

    public ComparacaoEstrategiaResultado(
        string nomeEstrategia,
        BacktestingResultado resultado)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nomeEstrategia);
        ArgumentNullException.ThrowIfNull(resultado);

        NomeEstrategia = nomeEstrategia;
        Resultado = resultado;
    }

    private int ContarJogosComMinimoDeAcertos(
        int quantidadeMinima)
    {
        return Resultado
            .ObterDistribuicaoAcertos()
            .Where(item => item.Key >= quantidadeMinima)
            .Sum(item => item.Value);
    }
}