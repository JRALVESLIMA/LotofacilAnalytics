using FluentAssertions;
using LotofacilAnalytics.Backtesting.Models;

namespace LotofacilAnalytics.Backtesting.Tests;

public class ResultadoJogoTests
{
    [Fact]
    public void Deve_armazenar_a_quantidade_de_acertos_do_jogo()
    {
        var resultado = new ResultadoJogo(
            Enumerable.Range(1, 15),
            8);

        resultado.Jogo.Should()
            .BeEquivalentTo(Enumerable.Range(1, 15));

        resultado.QuantidadeAcertos.Should().Be(8);
    }

    [Fact]
    public void Deve_rejeitar_jogo_com_quantidade_de_dezenas_diferente_de_15()
    {
        Action acao = () => new ResultadoJogo(
            Enumerable.Range(1, 14),
            10);

        acao.Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public void Deve_rejeitar_dezena_fora_do_intervalo_da_lotofacil(
        int dezenaInvalida)
    {
        var jogo = Enumerable.Range(1, 15).ToArray();

        jogo[0] = dezenaInvalida;

        Action acao = () => new ResultadoJogo(
            jogo,
            10);

        acao.Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void Deve_rejeitar_jogo_com_dezenas_duplicadas()
    {
        var jogo = Enumerable.Range(1, 14)
            .Append(14);

        Action acao = () => new ResultadoJogo(
            jogo,
            10);

        acao.Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(16)]
    public void Deve_rejeitar_quantidade_de_acertos_invalida(
        int quantidadeAcertos)
    {
        Action acao = () => new ResultadoJogo(
            Enumerable.Range(1, 15),
            quantidadeAcertos);

        acao.Should()
            .Throw<ArgumentException>();
    }
}