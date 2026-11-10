
using LotofacilAnalytics.GameGenerator.Comparison;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Reports;

namespace LotofacilAnalytics.GameGenerator.Tests.Comparison;

public class ComparacaoEstrategiasDetalhadaResultadoTests
{
    private static GeracaoJogosResultado CriarGeracao(
        int quantidade)
    {
        var jogos = Enumerable.Range(1, quantidade)
            .Select(inicio =>
                (IEnumerable<int>)Enumerable.Range(inicio, 15)
                    .ToArray())
            .ToArray();

        return new GeracaoJogosResultado(quantidade, jogos);
    }

    private static RelatorioGeracaoJogos CriarRelatorio(
        string nome,
        int quantidade)
    {
        return new RelatorioGeracaoJogos(
            nome,
            quantidade,
            quantidade,
            utilizouDiversificacao: false,
            quantidadeJogosOriginais: quantidade);
    }

    private static ComparacaoEstrategiasDetalhadaResultado
        CriarResultadoValido()
    {
        var geracaoA = CriarGeracao(3);
        var geracaoB = CriarGeracao(2);

        var relatorioA = CriarRelatorio("EstrategiaA", 3);
        var relatorioB = CriarRelatorio("EstrategiaB", 2);

        var comparacao = new ComparacaoColecoesResultado(
            "EstrategiaA",
            "EstrategiaB",
            3,
            2,
            1);

        return new ComparacaoEstrategiasDetalhadaResultado(
            geracaoA,
            geracaoB,
            relatorioA,
            relatorioB,
            comparacao);
    }

    [Fact]
    public void Construtor_ComDadosValidos_DeveArmazenarResultados()
    {
        var resultado = CriarResultadoValido();

        Assert.Equal(3, resultado.GeracaoA.QuantidadeGerada);
        Assert.Equal(2, resultado.GeracaoB.QuantidadeGerada);

        Assert.Equal(
            "EstrategiaA",
            resultado.RelatorioA.NomeEstrategia);

        Assert.Equal(
            "EstrategiaB",
            resultado.RelatorioB.NomeEstrategia);

        Assert.Equal(1, resultado.Comparacao.QuantidadeJogosComuns);
    }

    [Fact]
    public void Construtor_ComGeracaoANula_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        Assert.Throws<ArgumentNullException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                null!,
                valido.GeracaoB,
                valido.RelatorioA,
                valido.RelatorioB,
                valido.Comparacao));
    }

    [Fact]
    public void Construtor_ComGeracaoBNula_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        Assert.Throws<ArgumentNullException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                valido.GeracaoA,
                null!,
                valido.RelatorioA,
                valido.RelatorioB,
                valido.Comparacao));
    }

    [Fact]
    public void Construtor_ComRelatorioANulo_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        Assert.Throws<ArgumentNullException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                valido.GeracaoA,
                valido.GeracaoB,
                null!,
                valido.RelatorioB,
                valido.Comparacao));
    }

    [Fact]
    public void Construtor_ComRelatorioBNulo_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        Assert.Throws<ArgumentNullException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                valido.GeracaoA,
                valido.GeracaoB,
                valido.RelatorioA,
                null!,
                valido.Comparacao));
    }

    [Fact]
    public void Construtor_ComComparacaoNula_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        Assert.Throws<ArgumentNullException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                valido.GeracaoA,
                valido.GeracaoB,
                valido.RelatorioA,
                valido.RelatorioB,
                null!));
    }

    [Fact]
    public void Construtor_ComQuantidadeGeradaInconsistente_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        var relatorioInvalido = CriarRelatorio(
            "EstrategiaA",
            2);

        Assert.Throws<ArgumentException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                valido.GeracaoA,
                valido.GeracaoB,
                relatorioInvalido,
                valido.RelatorioB,
                valido.Comparacao));
    }

    [Fact]
    public void Construtor_ComComparacaoMaiorQueGeracao_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        var comparacaoInvalida = new ComparacaoColecoesResultado(
            "EstrategiaA",
            "EstrategiaB",
            4,
            2,
            1);

        Assert.Throws<ArgumentException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                valido.GeracaoA,
                valido.GeracaoB,
                valido.RelatorioA,
                valido.RelatorioB,
                comparacaoInvalida));
    }

    [Fact]
    public void Construtor_ComNomesInconsistentes_DeveRejeitar()
    {
        var valido = CriarResultadoValido();

        var comparacaoInvalida = new ComparacaoColecoesResultado(
            "OutraEstrategia",
            "EstrategiaB",
            3,
            2,
            1);

        Assert.Throws<ArgumentException>(() =>
            new ComparacaoEstrategiasDetalhadaResultado(
                valido.GeracaoA,
                valido.GeracaoB,
                valido.RelatorioA,
                valido.RelatorioB,
                comparacaoInvalida));
    }
}