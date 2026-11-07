
namespace LotofacilAnalytics.Backtesting.Models;

public class ComparacaoPeriodoResultado
{
    public int NumeroInicial { get; }
    public int NumeroFinal { get; }
    public string NomeEstrategia { get; }
    public BacktestingResultado Resultado { get; }

    public int QuantidadeJogos => Resultado.QuantidadeJogos;

    public double MediaAcertos => Resultado.MediaAcertos;

    public int JogosCom11OuMaisAcertos =>
        Resultado.Rodadas
            .SelectMany(rodada => rodada.ResultadosJogos)
            .Count(jogo => jogo.QuantidadeAcertos >= 11);

    public ComparacaoPeriodoResultado(
        int numeroInicial,
        int numeroFinal,
        string nomeEstrategia,
        BacktestingResultado resultado)
    {
        if (numeroInicial <= 0 || numeroFinal < numeroInicial)
            throw new ArgumentException("Período inválido.");

        ArgumentException.ThrowIfNullOrWhiteSpace(nomeEstrategia);
        ArgumentNullException.ThrowIfNull(resultado);

        NumeroInicial = numeroInicial;
        NumeroFinal = numeroFinal;
        NomeEstrategia = nomeEstrategia;
        Resultado = resultado;
    }
}