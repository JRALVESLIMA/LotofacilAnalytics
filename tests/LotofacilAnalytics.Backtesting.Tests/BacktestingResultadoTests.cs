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

    [Fact]
    public void Deve_retornar_metricas_zeradas_quando_nao_houver_rodadas()
    {
        var resultado = new BacktestingResultado(
            Array.Empty<RodadaBacktesting>());

        resultado.QuantidadeRodadas.Should().Be(0);
        resultado.QuantidadeJogos.Should().Be(0);
        resultado.MediaAcertos.Should().Be(0);
        resultado.MaiorAcertos.Should().Be(0);
        resultado.MenorAcertos.Should().Be(0);
        resultado.QuantidadeJogosComAcertos(10).Should().Be(0);
        resultado.ObterDistribuicaoAcertos().Should().BeEmpty();
    }

    [Fact]
    public void Deve_calcular_media_ignorando_rodadas_sem_jogos()
    {
        var rodadas = new[]
        {
            CriarRodada(100),
            CriarRodadaComAcertos(101, 8, 10),
            CriarRodada(102),
            CriarRodadaComAcertos(103, 12)
        };

        var resultado = new BacktestingResultado(rodadas);

        resultado.QuantidadeRodadas.Should().Be(4);
        resultado.QuantidadeJogos.Should().Be(3);
        resultado.MediaAcertos.Should().Be(10);
    }

    [Fact]
    public void Deve_calcular_a_variancia_populacional_dos_acertos()
    {
        var rodada = CriarRodadaComAcertos(
            100, 7, 8, 9, 10, 11);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        resultado.VarianciaAcertos.Should().BeApproximately(2.0, 0.0001);
    }

    [Fact]
    public void Deve_calcular_o_desvio_padrao_dos_acertos()
    {
        var rodada = CriarRodadaComAcertos(
            100, 7, 8, 9, 10, 11);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        resultado.DesvioPadraoAcertos
            .Should()
            .BeApproximately(Math.Sqrt(2), 0.0001);
    }

    [Fact]
    public void Deve_calcular_a_mediana_com_quantidade_impar_de_jogos()
    {
        var rodada = CriarRodadaComAcertos(
            100, 7, 8, 9, 10, 11);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        resultado.MedianaAcertos.Should().Be(9);
    }

    [Fact]
    public void Deve_calcular_a_mediana_com_quantidade_par_de_jogos()
    {
        var rodada = CriarRodadaComAcertos(
            100, 8, 9, 10, 11);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        resultado.MedianaAcertos.Should().Be(9.5);
    }

    [Fact]
    public void Deve_calcular_o_percentual_de_jogos_com_11_ou_mais_acertos()
    {
        var rodada = CriarRodadaComAcertos(
            100, 7, 8, 9, 10, 11);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        resultado.PercentualJogosCom11OuMaisAcertos
            .Should()
            .BeApproximately(20.0, 0.0001);
    }

    [Fact]
    public void Deve_retornar_variancia_e_desvio_padrao_zero_para_acertos_iguais()
    {
        var rodada = CriarRodadaComAcertos(
            100, 9, 9, 9);

        var resultado = new BacktestingResultado(
            new[] { rodada });

        resultado.VarianciaAcertos.Should().Be(0);
        resultado.DesvioPadraoAcertos.Should().Be(0);
    }

    [Fact]
    public void Deve_retornar_indicadores_estatisticos_zerados_sem_jogos()
    {
        var resultado = new BacktestingResultado(
            new[] { CriarRodada(100) });

        resultado.VarianciaAcertos.Should().Be(0);
        resultado.DesvioPadraoAcertos.Should().Be(0);
        resultado.MedianaAcertos.Should().Be(0);
        resultado.PercentualJogosCom11OuMaisAcertos.Should().Be(0);
    }

    [Fact]
    public void Deve_calcular_indicadores_ignorando_rodadas_sem_jogos()
    {
        var rodadas = new[]
        {
            CriarRodada(100),
            CriarRodadaComAcertos(101, 8, 10),
            CriarRodada(102),
            CriarRodadaComAcertos(103, 12)
        };

        var resultado = new BacktestingResultado(rodadas);

        resultado.QuantidadeJogos.Should().Be(3);
        resultado.MediaAcertos.Should().Be(10);
        resultado.VarianciaAcertos
            .Should()
            .BeApproximately(8.0 / 3.0, 0.0001);
        resultado.DesvioPadraoAcertos
            .Should()
            .BeApproximately(Math.Sqrt(8.0 / 3.0), 0.0001);
        resultado.MedianaAcertos.Should().Be(10);
        resultado.PercentualJogosCom11OuMaisAcertos
            .Should()
            .BeApproximately(100.0 / 3.0, 0.0001);
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
