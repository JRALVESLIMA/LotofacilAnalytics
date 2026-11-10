
using LotofacilAnalytics.GameGenerator.Comparison;

namespace LotofacilAnalytics.GameGenerator.Tests.Comparison;

public class ComparacaoJogosResultadoTests
{
    [Fact]
    public void Construtor_DeveArmazenarNomesDasEstrategias()
    {
        var resultado = new ComparacaoJogosResultado(
            "EstrategiaFrequenciaDezenas",
            "EstrategiaFrequenciaRecente",
            10);

        Assert.Equal(
            "EstrategiaFrequenciaDezenas",
            resultado.NomeEstrategiaA);

        Assert.Equal(
            "EstrategiaFrequenciaRecente",
            resultado.NomeEstrategiaB);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(15, 0)]
    public void Construtor_DeveCalcularDezenasExclusivas(
        int comuns,
        int exclusivas)
    {
        var resultado = new ComparacaoJogosResultado(
            "EstrategiaA",
            "EstrategiaB",
            comuns);

        Assert.Equal(
            comuns,
            resultado.QuantidadeDezenasComuns);

        Assert.Equal(
            exclusivas,
            resultado.QuantidadeDezenasExclusivasA);

        Assert.Equal(
            exclusivas,
            resultado.QuantidadeDezenasExclusivasB);
    }

    [Fact]
    public void JogosIdenticos_ComQuinzeDezenasComuns_DeveRetornarTrue()
    {
        var resultado = new ComparacaoJogosResultado(
            "EstrategiaA",
            "EstrategiaB",
            15);

        Assert.True(resultado.JogosIdenticos);
    }

    [Fact]
    public void JogosIdenticos_ComMenosDeQuinzeDezenasComuns_DeveRetornarFalse()
    {
        var resultado = new ComparacaoJogosResultado(
            "EstrategiaA",
            "EstrategiaB",
            14);

        Assert.False(resultado.JogosIdenticos);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(16)]
    public void Construtor_DeveRejeitarQuantidadeComunsInvalida(
        int quantidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ComparacaoJogosResultado(
                "EstrategiaA",
                "EstrategiaB",
                quantidade));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_DeveRejeitarNomeEstrategiaAInvalido(
        string? nome)
    {
        Assert.Throws<ArgumentException>(() =>
            new ComparacaoJogosResultado(
                nome!,
                "EstrategiaB",
                10));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_DeveRejeitarNomeEstrategiaBInvalido(
        string? nome)
    {
        Assert.Throws<ArgumentException>(() =>
            new ComparacaoJogosResultado(
                "EstrategiaA",
                nome!,
                10));
    }
}