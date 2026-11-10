
namespace LotofacilAnalytics.GameGenerator.Comparison;

public sealed class ComparacaoColecoesResultado
{
    public string NomeEstrategiaA { get; }
    public string NomeEstrategiaB { get; }

    public int QuantidadeJogosA { get; }
    public int QuantidadeJogosB { get; }

    public int QuantidadeJogosComuns { get; }

    public int QuantidadeJogosExclusivosA =>
        QuantidadeJogosA - QuantidadeJogosComuns;

    public int QuantidadeJogosExclusivosB =>
        QuantidadeJogosB - QuantidadeJogosComuns;

    public ComparacaoColecoesResultado(
        string nomeEstrategiaA,
        string nomeEstrategiaB,
        int quantidadeJogosA,
        int quantidadeJogosB,
        int quantidadeJogosComuns)
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

        if (quantidadeJogosA < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeJogosA));
        }

        if (quantidadeJogosB < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeJogosB));
        }

        if (quantidadeJogosComuns < 0 ||
            quantidadeJogosComuns > Math.Min(
                quantidadeJogosA,
                quantidadeJogosB))
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeJogosComuns));
        }

        NomeEstrategiaA = nomeEstrategiaA;
        NomeEstrategiaB = nomeEstrategiaB;

        QuantidadeJogosA = quantidadeJogosA;
        QuantidadeJogosB = quantidadeJogosB;

        QuantidadeJogosComuns = quantidadeJogosComuns;
    }
}