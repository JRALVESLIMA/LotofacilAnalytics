
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Reports;

namespace LotofacilAnalytics.GameGenerator.Comparison;

public sealed class ComparacaoEstrategiasDetalhadaResultado
{
    public GeracaoJogosResultado GeracaoA { get; }

    public GeracaoJogosResultado GeracaoB { get; }

    public RelatorioGeracaoJogos RelatorioA { get; }

    public RelatorioGeracaoJogos RelatorioB { get; }

    public ComparacaoColecoesResultado Comparacao { get; }

    public ComparacaoEstrategiasDetalhadaResultado(
        GeracaoJogosResultado geracaoA,
        GeracaoJogosResultado geracaoB,
        RelatorioGeracaoJogos relatorioA,
        RelatorioGeracaoJogos relatorioB,
        ComparacaoColecoesResultado comparacao)
    {
        ArgumentNullException.ThrowIfNull(geracaoA);
        ArgumentNullException.ThrowIfNull(geracaoB);
        ArgumentNullException.ThrowIfNull(relatorioA);
        ArgumentNullException.ThrowIfNull(relatorioB);
        ArgumentNullException.ThrowIfNull(comparacao);

        if (relatorioA.QuantidadeGerada != geracaoA.QuantidadeGerada)
        {
            throw new ArgumentException(
                "O relatório A deve corresponder à geração A.",
                nameof(relatorioA));
        }

        if (relatorioB.QuantidadeGerada != geracaoB.QuantidadeGerada)
        {
            throw new ArgumentException(
                "O relatório B deve corresponder à geração B.",
                nameof(relatorioB));
        }

        if (relatorioA.QuantidadeSolicitada !=
            geracaoA.QuantidadeSolicitada)
        {
            throw new ArgumentException(
                "A quantidade solicitada no relatório A é inconsistente.",
                nameof(relatorioA));
        }

        if (relatorioB.QuantidadeSolicitada !=
            geracaoB.QuantidadeSolicitada)
        {
            throw new ArgumentException(
                "A quantidade solicitada no relatório B é inconsistente.",
                nameof(relatorioB));
        }

        if (comparacao.QuantidadeJogosA > geracaoA.QuantidadeGerada ||
            comparacao.QuantidadeJogosB > geracaoB.QuantidadeGerada)
        {
            throw new ArgumentException(
                "A comparação não pode conter mais combinações " +
                "distintas do que jogos gerados.",
                nameof(comparacao));
        }

        if (comparacao.NomeEstrategiaA != relatorioA.NomeEstrategia ||
            comparacao.NomeEstrategiaB != relatorioB.NomeEstrategia)
        {
            throw new ArgumentException(
                "Os nomes das estratégias devem corresponder " +
                "aos respectivos relatórios.",
                nameof(comparacao));
        }

        GeracaoA = geracaoA;
        GeracaoB = geracaoB;
        RelatorioA = relatorioA;
        RelatorioB = relatorioB;
        Comparacao = comparacao;
    }
}