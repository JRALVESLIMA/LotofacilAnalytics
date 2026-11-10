
namespace LotofacilAnalytics.GameGenerator.Reports;

public sealed class RelatorioGeracaoJogos
{
    public string NomeEstrategia { get; }

    public int QuantidadeSolicitada { get; }

    public int QuantidadeGerada { get; }

    public bool UtilizouDiversificacao { get; }

    public int QuantidadeJogosOriginais { get; }

    public int QuantidadeJogosDiversificados { get; }

    public RelatorioGeracaoJogos(
        string nomeEstrategia,
        int quantidadeSolicitada,
        int quantidadeGerada,
        bool utilizouDiversificacao,
        int quantidadeJogosOriginais)
    {
        if (string.IsNullOrWhiteSpace(nomeEstrategia))
        {
            throw new ArgumentException(
                "O nome da estratégia é obrigatório.",
                nameof(nomeEstrategia));
        }

        if (quantidadeSolicitada <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeSolicitada));
        }

        if (quantidadeGerada < 0 ||
            quantidadeGerada > quantidadeSolicitada)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeGerada));
        }

        if (quantidadeJogosOriginais < 0 ||
            quantidadeJogosOriginais > quantidadeGerada)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeJogosOriginais));
        }

        if (!utilizouDiversificacao &&
            quantidadeJogosOriginais != quantidadeGerada)
        {
            throw new ArgumentException(
                "Sem diversificação, todos os jogos devem ser originais.");
        }

        if (utilizouDiversificacao &&
            quantidadeJogosOriginais != 1)
        {
            throw new ArgumentException(
                "A política atual de diversificação exige um jogo-base.");
        }

        NomeEstrategia = nomeEstrategia;
        QuantidadeSolicitada = quantidadeSolicitada;
        QuantidadeGerada = quantidadeGerada;
        UtilizouDiversificacao = utilizouDiversificacao;
        QuantidadeJogosOriginais = quantidadeJogosOriginais;

        QuantidadeJogosDiversificados =
            quantidadeGerada - quantidadeJogosOriginais;
    }
}