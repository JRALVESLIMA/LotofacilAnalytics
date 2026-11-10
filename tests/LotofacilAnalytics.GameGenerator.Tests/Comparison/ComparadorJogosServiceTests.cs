
using LotofacilAnalytics.GameGenerator.Comparison;

namespace LotofacilAnalytics.GameGenerator.Tests.Comparison;

public class ComparadorJogosServiceTests
{
    private readonly ComparadorJogosService _service = new();

    [Fact]
    public void Comparar_JogosIdenticos_DeveRetornarQuinzeDezenasComuns()
    {
        var jogoA = Enumerable.Range(1, 15).ToArray();
        var jogoB = Enumerable.Range(1, 15).ToArray();

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogoA,
            "EstrategiaB",
            jogoB);

        Assert.Equal(15, resultado.QuantidadeDezenasComuns);
        Assert.Equal(0, resultado.QuantidadeDezenasExclusivasA);
        Assert.Equal(0, resultado.QuantidadeDezenasExclusivasB);
        Assert.True(resultado.JogosIdenticos);
    }

    [Fact]
    public void Comparar_JogosComCincoDezenasComuns_DeveRetornarCinco()
    {
        var jogoA = Enumerable.Range(1, 15).ToArray();
        var jogoB = Enumerable.Range(11, 15).ToArray();

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogoA,
            "EstrategiaB",
            jogoB);

        Assert.Equal(5, resultado.QuantidadeDezenasComuns);
        Assert.Equal(10, resultado.QuantidadeDezenasExclusivasA);
        Assert.Equal(10, resultado.QuantidadeDezenasExclusivasB);
        Assert.False(resultado.JogosIdenticos);
    }

    [Fact]
    public void Comparar_DeveIgnorarOrdemDasDezenas()
    {
        var jogoA = Enumerable.Range(1, 15).ToArray();
        var jogoB = Enumerable.Range(1, 15).Reverse().ToArray();

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogoA,
            "EstrategiaB",
            jogoB);

        Assert.Equal(15, resultado.QuantidadeDezenasComuns);
        Assert.True(resultado.JogosIdenticos);
    }

    [Fact]
    public void Comparar_DevePreservarNomesDasEstrategias()
    {
        var jogoA = Enumerable.Range(1, 15).ToArray();
        var jogoB = Enumerable.Range(11, 15).ToArray();

        var resultado = _service.Comparar(
            "FrequenciaGeral",
            jogoA,
            "FrequenciaRecente",
            jogoB);

        Assert.Equal("FrequenciaGeral", resultado.NomeEstrategiaA);
        Assert.Equal("FrequenciaRecente", resultado.NomeEstrategiaB);
    }

    [Fact]
    public void Comparar_ComJogoANulo_DeveLancarExcecao()
    {
        var jogoB = Enumerable.Range(1, 15).ToArray();

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                "EstrategiaA",
                null!,
                "EstrategiaB",
                jogoB));
    }

    [Fact]
    public void Comparar_ComJogoBNulo_DeveLancarExcecao()
    {
        var jogoA = Enumerable.Range(1, 15).ToArray();

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogoA,
                "EstrategiaB",
                null!));
    }

    [Theory]
    [InlineData(14)]
    [InlineData(16)]
    public void Comparar_ComQuantidadeDeDezenasInvalida_DeveRejeitar(
        int quantidade)
    {
        var jogoA = Enumerable.Range(1, quantidade).ToArray();
        var jogoB = Enumerable.Range(1, 15).ToArray();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogoA,
                "EstrategiaB",
                jogoB));
    }

    [Fact]
    public void Comparar_ComDezenasDuplicadas_DeveRejeitar()
    {
        var jogoA = Enumerable.Repeat(1, 15).ToArray();
        var jogoB = Enumerable.Range(1, 15).ToArray();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogoA,
                "EstrategiaB",
                jogoB));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public void Comparar_ComDezenasForaDoIntervalo_DeveRejeitar(
        int dezenaInvalida)
    {
        var jogoA = Enumerable.Range(1, 14)
            .Append(dezenaInvalida)
            .ToArray();

        var jogoB = Enumerable.Range(1, 15).ToArray();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogoA,
                "EstrategiaB",
                jogoB));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Comparar_ComNomeEstrategiaAInvalido_DeveRejeitar(
        string? nome)
    {
        var jogo = Enumerable.Range(1, 15).ToArray();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                nome!,
                jogo,
                "EstrategiaB",
                jogo));
    }
}