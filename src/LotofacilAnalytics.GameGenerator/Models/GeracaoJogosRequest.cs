
namespace LotofacilAnalytics.GameGenerator.Models;

public sealed class GeracaoJogosRequest
{
    public const int QuantidadeMaximaJogos = 100;

    public int QuantidadeJogos { get; }

    public bool PermitirJogosRepetidos { get; }

    public bool DiversificarJogos { get; }

    public int SementeDiversificacao { get; }

    public GeracaoJogosRequest(
        int quantidadeJogos,
        bool permitirJogosRepetidos = false,
        bool diversificarJogos = false,
        int sementeDiversificacao = 12345)
    {
        if (quantidadeJogos <= 0 ||
            quantidadeJogos > QuantidadeMaximaJogos)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeJogos),
                $"A quantidade de jogos deve estar entre 1 e " +
                $"{QuantidadeMaximaJogos}.");
        }

        if (diversificarJogos && permitirJogosRepetidos)
        {
            throw new ArgumentException(
                "A diversificação não permite jogos repetidos.");
        }

        QuantidadeJogos = quantidadeJogos;
        PermitirJogosRepetidos = permitirJogosRepetidos;
        DiversificarJogos = diversificarJogos;
        SementeDiversificacao = sementeDiversificacao;
    }
}