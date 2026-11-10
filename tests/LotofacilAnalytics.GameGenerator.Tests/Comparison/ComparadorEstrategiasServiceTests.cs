
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.GameGenerator.Comparison;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Tests.Fixtures;

namespace LotofacilAnalytics.GameGenerator.Tests.Comparison;

public class ComparadorEstrategiasServiceTests
{
    private readonly ComparadorEstrategiasService _service = new();

    [Fact]
    public void Comparar_ComHistoricoNulo_DeveRejeitar()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                null!,
                "EstrategiaA",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                "EstrategiaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Comparar_ComEstrategiaANula_DeveRejeitar()
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                historico,
                "EstrategiaA",
                null!,
                new GeracaoJogosRequest(1),
                "EstrategiaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Comparar_ComEstrategiaBNula_DeveRejeitar()
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                historico,
                "EstrategiaA",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                "EstrategiaB",
                null!,
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Comparar_ComRequestANulo_DeveRejeitar()
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                historico,
                "EstrategiaA",
                new EstrategiaDezenasFixas(),
                null!,
                "EstrategiaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Comparar_ComRequestBNulo_DeveRejeitar()
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                historico,
                "EstrategiaA",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                "EstrategiaB",
                new EstrategiaDezenasFixas(),
                null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Comparar_ComNomeEstrategiaAInvalido_DeveRejeitar(
        string? nome)
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                historico,
                nome!,
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                "EstrategiaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Comparar_ComNomeEstrategiaBInvalido_DeveRejeitar(
        string? nome)
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                historico,
                "EstrategiaA",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                nome!,
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Comparar_ComHistoricoVazio_DeveRejeitar()
    {
        var historico = Array.Empty<Concurso>();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                historico,
                "EstrategiaA",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1),
                "EstrategiaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Comparar_ComDuasEstrategiasFixas_DeveRetornarJogoComum()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _service.Comparar(
            historico,
            "FixaA",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1),
            "FixaB",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1));

        Assert.Equal("FixaA", resultado.NomeEstrategiaA);
        Assert.Equal("FixaB", resultado.NomeEstrategiaB);

        Assert.Equal(1, resultado.QuantidadeJogosA);
        Assert.Equal(1, resultado.QuantidadeJogosB);
        Assert.Equal(1, resultado.QuantidadeJogosComuns);

        Assert.Equal(0, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ComEstrategiasFixasDiversificadas_DeveReconhecerJogosIguais()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var requestA = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            diversificarJogos: true,
            sementeDiversificacao: 12345);

        var requestB = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            diversificarJogos: true,
            sementeDiversificacao: 12345);

        var resultado = _service.Comparar(
            historico,
            "FixaA",
            new EstrategiaDezenasFixas(),
            requestA,
            "FixaB",
            new EstrategiaDezenasFixas(),
            requestB);

        Assert.Equal(5, resultado.QuantidadeJogosA);
        Assert.Equal(5, resultado.QuantidadeJogosB);
        Assert.Equal(5, resultado.QuantidadeJogosComuns);

        Assert.Equal(0, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ComQuantidadesDiferentes_DeveCalcularCorretamente()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var requestA = new GeracaoJogosRequest(
            quantidadeJogos: 3,
            diversificarJogos: true,
            sementeDiversificacao: 12345);

        var requestB = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            diversificarJogos: true,
            sementeDiversificacao: 12345);

        var resultado = _service.Comparar(
            historico,
            "FixaA",
            new EstrategiaDezenasFixas(),
            requestA,
            "FixaB",
            new EstrategiaDezenasFixas(),
            requestB);

        Assert.Equal(3, resultado.QuantidadeJogosA);
        Assert.Equal(5, resultado.QuantidadeJogosB);

        Assert.Equal(3, resultado.QuantidadeJogosComuns);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(2, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ComEstrategiaFixaEAleatoria_DeveRetornarComparacaoValida()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _service.Comparar(
            historico,
            "Fixa",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1),
            "Aleatoria",
            new EstrategiaAleatoria(semente: 12345),
            new GeracaoJogosRequest(1));

        Assert.Equal(1, resultado.QuantidadeJogosA);
        Assert.Equal(1, resultado.QuantidadeJogosB);

        Assert.InRange(resultado.QuantidadeJogosComuns, 0, 1);

        Assert.Equal(
            1 - resultado.QuantidadeJogosComuns,
            resultado.QuantidadeJogosExclusivosA);

        Assert.Equal(
            1 - resultado.QuantidadeJogosComuns,
            resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ComEstrategiaDeFrequencia_DeveGerarComparacaoValida()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _service.Comparar(
            historico,
            "Fixa",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1),
            "Frequencia",
            new EstrategiaFrequenciaDezenas(),
            new GeracaoJogosRequest(1));

        Assert.Equal(1, resultado.QuantidadeJogosA);
        Assert.Equal(1, resultado.QuantidadeJogosB);

        Assert.InRange(resultado.QuantidadeJogosComuns, 0, 1);

        Assert.Equal(
            1 - resultado.QuantidadeJogosComuns,
            resultado.QuantidadeJogosExclusivosA);

        Assert.Equal(
            1 - resultado.QuantidadeJogosComuns,
            resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ComEstrategiaDeFrequenciaRecente_DeveGerarComparacaoValida()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _service.Comparar(
            historico,
            "Fixa",
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1),
            "FrequenciaRecente",
            new EstrategiaFrequenciaRecente(quantidadeConcursos: 2),
            new GeracaoJogosRequest(1));

        Assert.Equal(1, resultado.QuantidadeJogosA);
        Assert.Equal(1, resultado.QuantidadeJogosB);

        Assert.InRange(resultado.QuantidadeJogosComuns, 0, 1);
    }

    [Fact]
    public void Comparar_SemDiversificacaoEComJogosFixosInsuficientes_DeveRejeitar()
    {
        var historico = ConcursoFixture.CriarHistorico();

        Assert.Throws<InvalidOperationException>(() =>
            _service.Comparar(
                historico,
                "FixaA",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(3),
                "FixaB",
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }
}