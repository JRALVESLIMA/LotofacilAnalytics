
using LotofacilAnalytics.GameGenerator.Comparison;

namespace LotofacilAnalytics.GameGenerator.Tests.Comparison;

public class ComparacaoColecoesResultadoTests
{
    [Fact]
    public void Construtor_DeveArmazenarDadosCorretamente()
    {
        var resultado = new ComparacaoColecoesResultado(
            "EstrategiaA",
            "EstrategiaB",
            10,
            8,
            3);

        Assert.Equal("EstrategiaA", resultado.NomeEstrategiaA);
        Assert.Equal("EstrategiaB", resultado.NomeEstrategiaB);
        Assert.Equal(10, resultado.QuantidadeJogosA);
        Assert.Equal(8, resultado.QuantidadeJogosB);
        Assert.Equal(3, resultado.QuantidadeJogosComuns);
    }

    [Fact]
    public void Construtor_DeveCalcularJogosExclusivos()
    {
        var resultado = new ComparacaoColecoesResultado(
            "EstrategiaA",
            "EstrategiaB",
            10,
            8,
            3);

        Assert.Equal(7, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(5, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Construtor_ComColecoesIdenticas_DeveRetornarZeroExclusivos()
    {
        var resultado = new ComparacaoColecoesResultado(
            "EstrategiaA",
            "EstrategiaB",
            5,
            5,
            5);

        Assert.Equal(5, resultado.QuantidadeJogosComuns);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Construtor_ComColecoesVazias_DeveAceitar()
    {
        var resultado = new ComparacaoColecoesResultado(
            "EstrategiaA",
            "EstrategiaB",
            0,
            0,
            0);

        Assert.Equal(0, resultado.QuantidadeJogosA);
        Assert.Equal(0, resultado.QuantidadeJogosB);
        Assert.Equal(0, resultado.QuantidadeJogosComuns);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosB);
    }

    [Theory]
    [InlineData(-1, 5, 0)]
    [InlineData(5, -1, 0)]
    [InlineData(5, 5, -1)]
    [InlineData(5, 5, 6)]
    [InlineData(3, 10, 4)]
    public void Construtor_ComQuantidadesInvalidas_DeveRejeitar(
        int quantidadeA,
        int quantidadeB,
        int comuns)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ComparacaoColecoesResultado(
                "EstrategiaA",
                "EstrategiaB",
                quantidadeA,
                quantidadeB,
                comuns));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_ComNomeEstrategiaAInvalido_DeveRejeitar(
        string? nome)
    {
        Assert.Throws<ArgumentException>(() =>
            new ComparacaoColecoesResultado(
                nome!,
                "EstrategiaB",
                5,
                5,
                2));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_ComNomeEstrategiaBInvalido_DeveRejeitar(
        string? nome)
    {
        Assert.Throws<ArgumentException>(() =>
            new ComparacaoColecoesResultado(
                "EstrategiaA",
                nome!,
                5,
                5,
                2));
    }
}