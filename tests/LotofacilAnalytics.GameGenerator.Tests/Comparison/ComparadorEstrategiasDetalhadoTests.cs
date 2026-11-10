
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.GameGenerator.Comparison;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Tests.Fixtures;

namespace LotofacilAnalytics.GameGenerator.Tests.Comparison;

public class ComparadorEstrategiasDetalhadoTests
{
    private readonly ComparadorEstrategiasService _service = new();

    [Fact]
    public void CompararDetalhado_ComDuasEstrategiasFixas_DeveRetornarDadosConsistentes()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _service.CompararDetalhado(
            historico,
            "FixaA",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1),
            "FixaB",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1));

        Assert.Single(resultado.GeracaoA.Jogos);
        Assert.Single(resultado.GeracaoB.Jogos);

        Assert.Equal(1, resultado.RelatorioA.QuantidadeGerada);
        Assert.Equal(1, resultado.RelatorioB.QuantidadeGerada);

        Assert.Equal(1, resultado.Comparacao.QuantidadeJogosComuns);
        Assert.Equal(0, resultado.Comparacao.QuantidadeJogosExclusivosA);
        Assert.Equal(0, resultado.Comparacao.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void CompararDetalhado_ComDiversificacao_DeveGerarRelatoriosCorretos()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var request = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            diversificarJogos: true,
            sementeDiversificacao: 12345);

        var resultado = _service.CompararDetalhado(
            historico,
            "FixaA",
            new EstrategiaDezenasFixas(),
            request,
            "FixaB",
            new EstrategiaDezenasFixas(),
            request);

        Assert.Equal(5, resultado.GeracaoA.QuantidadeGerada);
        Assert.Equal(5, resultado.GeracaoB.QuantidadeGerada);

        Assert.True(resultado.RelatorioA.UtilizouDiversificacao);
        Assert.True(resultado.RelatorioB.UtilizouDiversificacao);

        Assert.Equal(1, resultado.RelatorioA.QuantidadeJogosOriginais);
        Assert.Equal(1, resultado.RelatorioB.QuantidadeJogosOriginais);

        Assert.Equal(4, resultado.RelatorioA.QuantidadeJogosDiversificados);
        Assert.Equal(4, resultado.RelatorioB.QuantidadeJogosDiversificados);

        Assert.Equal(5, resultado.Comparacao.QuantidadeJogosComuns);
    }

    [Fact]
    public void CompararDetalhado_ComQuantidadesDiferentes_DeveManterConsistencia()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _service.CompararDetalhado(
            historico,
            "FixaA",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(
                quantidadeJogos: 3,
                diversificarJogos: true,
                sementeDiversificacao: 12345),
            "FixaB",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(
                quantidadeJogos: 5,
                diversificarJogos: true,
                sementeDiversificacao: 12345));

        Assert.Equal(3, resultado.RelatorioA.QuantidadeGerada);
        Assert.Equal(5, resultado.RelatorioB.QuantidadeGerada);

        Assert.Equal(3, resultado.Comparacao.QuantidadeJogosComuns);
        Assert.Equal(0, resultado.Comparacao.QuantidadeJogosExclusivosA);
        Assert.Equal(2, resultado.Comparacao.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void CompararDetalhado_ComEstrategiaAleatoria_DeveCompararJogosGerados()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _service.CompararDetalhado(
            historico,
            "Fixa",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1),
            "Aleatoria",
            new EstrategiaAleatoria(semente: 12345),
            new GeracaoJogosRequest(1));

        var comparador = new ComparadorColecoesService();

        var comparacaoEsperada = comparador.Comparar(
            "Fixa",
            resultado.GeracaoA.Jogos,
            "Aleatoria",
            resultado.GeracaoB.Jogos);

        Assert.Equal(
            comparacaoEsperada.QuantidadeJogosComuns,
            resultado.Comparacao.QuantidadeJogosComuns);

        Assert.Equal(
            comparacaoEsperada.QuantidadeJogosExclusivosA,
            resultado.Comparacao.QuantidadeJogosExclusivosA);

        Assert.Equal(
            comparacaoEsperada.QuantidadeJogosExclusivosB,
            resultado.Comparacao.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void CompararDetalhado_ComHistoricoVazio_DeveRejeitar()
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentException>(() =>
            _service.CompararDetalhado(
                historico,
                "FixaA",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                "FixaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void CompararDetalhado_ComNomeInvalido_DeveRejeitar()
    {
        var historico = ConcursoFixture.CriarHistorico();

        Assert.Throws<ArgumentException>(() =>
            _service.CompararDetalhado(
                historico,
                "",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                "FixaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }
}