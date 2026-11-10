
namespace LotofacilAnalytics.GameGenerator.Comparison;

public sealed class ComparacaoJogosResultado
{
    public string NomeEstrategiaA { get; }

    public string NomeEstrategiaB { get; }

    public int QuantidadeDezenasComuns { get; }

    public int QuantidadeDezenasExclusivasA { get; }

    public int QuantidadeDezenasExclusivasB { get; }

    public bool JogosIdenticos => QuantidadeDezenasComuns == 15;

    public ComparacaoJogosResultado(
        string nomeEstrategiaA,
        string nomeEstrategiaB,
        int quantidadeDezenasComuns)
    {
        if (string.IsNullOrWhiteSpace(nomeEstrategiaA))
        {
            throw new ArgumentException(
                "O nome da estratégia A é obrigatório.",
                nameof(nomeEstrategiaA));
        }

        if (string.IsNullOrWhiteSpace(nomeEstrategiaB))
        {
            throw new ArgumentException(
                "O nome da estratégia B é obrigatório.",
                nameof(nomeEstrategiaB));
        }

        if (quantidadeDezenasComuns < 5 ||
            quantidadeDezenasComuns > 15)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeDezenasComuns),
                "Dois jogos válidos da Lotofácil devem " +
                "compartilhar entre 5 e 15 dezenas.");
        }

        NomeEstrategiaA = nomeEstrategiaA;
        NomeEstrategiaB = nomeEstrategiaB;

        QuantidadeDezenasComuns = quantidadeDezenasComuns;

        QuantidadeDezenasExclusivasA =
            15 - quantidadeDezenasComuns;

        QuantidadeDezenasExclusivasB =
            15 - quantidadeDezenasComuns;
    }
}