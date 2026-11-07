
using LotofacilAnalytics.Backtesting.Services;
using Xunit;

namespace LotofacilAnalytics.Backtesting.Tests.Services;

public class ProbabilidadeTeoricaServiceTests
{
    private readonly ProbabilidadeTeoricaService _service = new();

    [Fact]
    public void Deve_calcular_probabilidade_de_15_acertos()
    {
        // Arrange
        double esperado = 1.0 / 3268760;

        // Act
        double resultado = _service.CalcularProbabilidade(15);

        // Assert
        Assert.Equal(esperado, resultado, 12);
    }

    [Fact]
    public void Deve_calcular_probabilidade_de_11_acertos()
    {
        // Arrange
        double esperado =
            (1365.0 * 210) / 3268760;

        // Act
        double resultado = _service.CalcularProbabilidade(11);

        // Assert
        Assert.Equal(esperado, resultado, 12);
    }

    [Fact]
    public void Soma_das_probabilidades_deve_ser_igual_a_1()
    {
        // Act
        double soma = Enumerable
            .Range(5, 11)
            .Sum(acertos => _service.CalcularProbabilidade(acertos));

        // Assert
        Assert.Equal(1.0, soma, 10);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(16)]
    [InlineData(-1)]
    public void Deve_rejeitar_quantidade_de_acertos_invalida(
        int acertos)
    {
        // Act e Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _service.CalcularProbabilidade(acertos));
    }
}