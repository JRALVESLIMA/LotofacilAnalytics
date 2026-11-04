
using FluentAssertions;
using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests.Models;

public class ComparacaoEstrategiaResultadoTests
{
    [Fact]
    public void Deve_calcular_corretamente_indicadores_da_estrategia()
    {
        var resultado = CriarResultado(10, 11, 12, 13, 14, 15);

        var comparacao = new ComparacaoEstrategiaResultado(
            "Frequencia Geral",
            resultado);

        comparacao.NomeEstrategia.Should().Be("Frequencia Geral");
        comparacao.Resultado.Should().BeSameAs(resultado);

        comparacao.QuantidadeRodadas.Should().Be(1);
        comparacao.QuantidadeJogos.Should().Be(6);

        comparacao.MediaAcertos.Should().Be(12.5);
        comparacao.MaiorAcertos.Should().Be(15);
        comparacao.MenorAcertos.Should().Be(10);

        comparacao.JogosCom11OuMaisAcertos.Should().Be(5);
        comparacao.JogosCom13OuMaisAcertos.Should().Be(3);
    }

    [Fact]
    public void Deve_retornar_zero_quando_nao_existirem_jogos()
    {
        var resultado = new BacktestingResultado(
            Array.Empty<RodadaBacktesting>());

        var comparacao = new ComparacaoEstrategiaResultado(
            "Estrategia Vazia",
            resultado);

        comparacao.QuantidadeRodadas.Should().Be(0);
        comparacao.QuantidadeJogos.Should().Be(0);

        comparacao.MediaAcertos.Should().Be(0);
        comparacao.MaiorAcertos.Should().Be(0);
        comparacao.MenorAcertos.Should().Be(0);

        comparacao.JogosCom11OuMaisAcertos.Should().Be(0);
        comparacao.JogosCom13OuMaisAcertos.Should().Be(0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_rejeitar_nome_de_estrategia_invalido(
        string? nome)
    {
        var resultado = new BacktestingResultado(
            Array.Empty<RodadaBacktesting>());

        var acao = () => new ComparacaoEstrategiaResultado(
            nome!,
            resultado);

        acao.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Deve_rejeitar_resultado_nulo()
    {
        var acao = () => new ComparacaoEstrategiaResultado(
            "Frequencia Geral",
            null!);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_contar_corretamente_jogos_abaixo_dos_limites()
    {
        var resultado = CriarResultado(7, 8, 9, 10);

        var comparacao = new ComparacaoEstrategiaResultado(
            "Estrategia Teste",
            resultado);

        comparacao.JogosCom11OuMaisAcertos.Should().Be(0);
        comparacao.JogosCom13OuMaisAcertos.Should().Be(0);
    }

    private static BacktestingResultado CriarResultado(
        params int[] quantidadesAcertos)
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            Enumerable.Range(1, 15).ToArray());

        var resultadosJogos = quantidadesAcertos
            .Select(acertos => new ResultadoJogo(
                Enumerable.Range(1, 15),
                acertos))
            .ToArray();

        var rodada = new RodadaBacktesting(
            concurso,
            Array.Empty<Concurso>(),
            resultadosJogos);

        return new BacktestingResultado(
            new[] { rodada });
    }
}