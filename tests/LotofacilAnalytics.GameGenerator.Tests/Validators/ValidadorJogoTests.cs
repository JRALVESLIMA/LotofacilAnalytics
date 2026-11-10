
using LotofacilAnalytics.GameGenerator.Validators;

namespace LotofacilAnalytics.GameGenerator.Tests.Validators;

public class ValidadorJogoTests
{
    private readonly ValidadorJogo _validador = new();

    [Fact]
    public void Validar_DeveAceitarJogoCom15DezenasValidas()
    {
        var dezenas = Enumerable.Range(1, 15).ToArray();

        var excecao = Record.Exception(() =>
            _validador.Validar(dezenas));

        Assert.Null(excecao);
    }

    [Theory]
    [InlineData(14)]
    [InlineData(16)]
    public void Validar_DeveRejeitarQuantidadeInvalida(
        int quantidade)
    {
        var dezenas = Enumerable.Range(1, quantidade).ToArray();

        Assert.Throws<ArgumentException>(() =>
            _validador.Validar(dezenas));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public void Validar_DeveRejeitarDezenaForaDoIntervalo(
        int dezenaInvalida)
    {
        var dezenas = Enumerable.Range(1, 15).ToArray();

        dezenas[0] = dezenaInvalida;

        Assert.Throws<ArgumentException>(() =>
            _validador.Validar(dezenas));
    }

    [Fact]
    public void Validar_DeveRejeitarDezenasRepetidas()
    {
        var dezenas = Enumerable.Range(1, 15).ToArray();

        dezenas[14] = 14;

        Assert.Throws<ArgumentException>(() =>
            _validador.Validar(dezenas));
    }

    [Fact]
    public void Validar_DeveRejeitarColecaoNula()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _validador.Validar(null!));
    }
}