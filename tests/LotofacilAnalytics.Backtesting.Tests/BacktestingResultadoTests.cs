using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Domain.Entities;
using FluentAssertions;

namespace LotofacilAnalytics.Backtesting.Tests;

public class BacktestingResultadoTests
{
    [Fact]
    public void Deve_informar_a_quantidade_de_rodadas_avaliadas()
    {
        var rodadas = new[]
        {
            CriarRodada(100),
            CriarRodada(101),
            CriarRodada(102)
        };

        var resultado = new BacktestingResultado(rodadas);

        resultado.QuantidadeRodadas.Should().Be(3);
    }

    [Fact]
    public void Deve_informar_a_quantidade_total_de_jogos_avaliados()
    {
        var rodadas = new[]
        {
            CriarRodada(100, 2),
            CriarRodada(101, 3),
            CriarRodada(102, 1)
        };

        var resultado = new BacktestingResultado(rodadas);

        resultado.QuantidadeJogos.Should().Be(6);
    }

    [Fact]
    public void Deve_calcular_a_media_de_acertos_dos_jogos_avaliados()
    {
        var rodadas = new[]
        {
            CriarRodadaComAcertos(100, 8, 10),
            CriarRodadaComAcertos(101, 12)
        };

        var resultado = new BacktestingResultado(rodadas);

        resultado.MediaAcertos.Should().Be(10);
    }

    [Fact]
    public void Deve_informar_o_maior_numero_de_acertos()
    {
        var rodadas = new[]
        {
            CriarRodadaComAcertos(100, 8, 10),
            CriarRodadaComAcertos(101, 12, 9)
        };

        var resultado = new BacktestingResultado(rodadas);

        resultado.MaiorAcertos.Should().Be(12);
    }

    [Fact]
    public void Deve_informar_o_menor_numero_de_acertos()
    {
        var rodadas = new[]
        {
            CriarRodadaComAcertos(100, 8, 10),
            CriarRodadaComAcertos(101, 12, 6)
        };

        var resultado = new BacktestingResultado(rodadas);

        resultado.MenorAcertos.Should().Be(6);
    }

    [Fact]
    public void Deve_informar_a_quantidade_de_jogos_com_determinada_quantidade_de_acertos()
    {
        var rodada = CriarRodadaComAcertos(
            1,
            8, 10, 10, 12);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        resultado.QuantidadeJogosComAcertos(10)
            .Should()
            .Be(2);
    }

    [Fact]
    public void Deve_informar_a_distribuicao_dos_acertos()
    {
        var rodada = CriarRodadaComAcertos(
            1,
            8, 10, 10, 12);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        var distribuicao = resultado.ObterDistribuicaoAcertos();

        distribuicao[8].Should().Be(1);
        distribuicao[10].Should().Be(2);
        distribuicao[12].Should().Be(1);
    }

    [Fact]
    public void Deve_retornar_distribuicao_vazia_quando_nao_houver_jogos_avaliados()
    {
        var rodada = CriarRodada(1);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        var distribuicao = resultado.ObterDistribuicaoAcertos();

        distribuicao.Should().BeEmpty();
    }

    private static RodadaBacktesting CriarRodada(
        int numeroConcurso,
        int quantidadeJogos = 0)
    {
        var concurso = new Concurso(
            numeroConcurso,
            new DateTime(2026, 1, 1),
            Enumerable.Range(1, 15));

        var resultadosJogos = Enumerable
            .Range(0, quantidadeJogos)
            .Select(_ => new ResultadoJogo(
                Enumerable.Range(1, 15),
                15))
            .ToArray();

        return new RodadaBacktesting(
            concurso,
            Array.Empty<Concurso>(),
            resultadosJogos);
    }

    private static RodadaBacktesting CriarRodadaComAcertos(
        int numeroConcurso,
        params int[] acertos)
    {
        var concurso = new Concurso(
            numeroConcurso,
            new DateTime(2026, 1, 1),
            Enumerable.Range(1, 15));

        var resultadosJogos = acertos
            .Select(quantidadeAcertos =>
                new ResultadoJogo(
                    Enumerable.Range(1, 15),
                    quantidadeAcertos))
            .ToArray();

        return new RodadaBacktesting(
            concurso,
            Array.Empty<Concurso>(),
            resultadosJogos);
    }
}
