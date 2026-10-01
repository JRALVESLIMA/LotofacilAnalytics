using FluentAssertions;
using LotofacilAnalytics.Backtesting.Services;

namespace LotofacilAnalytics.Backtesting.Tests;

public class AvaliadorJogoTests
{
    [Fact]
    public void Deve_calcular_a_quantidade_de_acertos_do_jogo()
    {
        var jogo = new[]
        {
            1, 2, 3, 4, 5,
            6, 7, 8, 9, 10,
            11, 12, 13, 14, 15
        };

        var resultadoReal = new[]
        {
            1, 3, 5, 7, 9,
            11, 13, 15, 17, 19,
            20, 21, 22, 23, 25
        };

        var avaliador = new AvaliadorJogo();

        var acertos = avaliador.CalcularAcertos(
            jogo,
            resultadoReal);

        acertos.Should().Be(8);
    }

    [Fact]
    public void Deve_retornar_cinco_quando_o_jogo_tiver_o_minimo_de_acertos()
    {
        var jogo = Enumerable.Range(1, 15);

        var resultadoReal = Enumerable.Range(11, 15);

        var avaliador = new AvaliadorJogo();

        var acertos = avaliador.CalcularAcertos(
            jogo,
            resultadoReal);

        acertos.Should().Be(5);
    }

    [Fact]
    public void Deve_retornar_quinze_quando_o_jogo_for_igual_ao_resultado_real()
    {
        var jogo = Enumerable.Range(1, 15);

        var resultadoReal = Enumerable.Range(1, 15);

        var avaliador = new AvaliadorJogo();

        var acertos = avaliador.CalcularAcertos(
            jogo,
            resultadoReal);

        acertos.Should().Be(15);
    }

    
}