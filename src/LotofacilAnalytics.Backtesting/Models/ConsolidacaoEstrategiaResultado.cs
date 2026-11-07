
namespace LotofacilAnalytics.Backtesting.Models;

public class ConsolidacaoEstrategiaResultado
{
    public string NomeEstrategia { get; }

    public int QuantidadePeriodos { get; }

    public BacktestingResultado Resultado { get; }

    public int QuantidadeJogos => Resultado.QuantidadeJogos;

    public double MediaAcertos => Resultado.MediaAcertos;

    public double MedianaAcertos => Resultado.MedianaAcertos;

    public double DesvioPadraoAcertos =>
        Resultado.DesvioPadraoAcertos;

    public double VarianciaAcertos =>
        Resultado.VarianciaAcertos;

    public double PercentualJogosCom11OuMaisAcertos =>
        Resultado.PercentualJogosCom11OuMaisAcertos;

    public ConsolidacaoEstrategiaResultado(
        string nomeEstrategia,
        int quantidadePeriodos,
        BacktestingResultado resultado)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nomeEstrategia);
        ArgumentNullException.ThrowIfNull(resultado);

        if (quantidadePeriodos <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadePeriodos));
        }

        NomeEstrategia = nomeEstrategia;
        QuantidadePeriodos = quantidadePeriodos;
        Resultado = resultado;
    }
}