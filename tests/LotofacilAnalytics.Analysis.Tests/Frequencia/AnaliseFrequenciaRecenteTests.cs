using LotofacilAnalytics.Analysis.Frequencia;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Frequencia;

public class AnaliseFrequenciaRecenteTests
{
    [Fact]
    public void DeveCalcularFrequenciaDosUltimosConcursos()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 3, 4, 5,
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseFrequenciaRecente();

        var resultado = analise.Calcular(concursos, 2);

        Assert.Equal(2, resultado[1]);
        Assert.Equal(2, resultado[5]);
        Assert.Equal(1, resultado[6]);
        Assert.Equal(1, resultado[16]);
        Assert.Equal(2, resultado[25]);
    }

    [Fact]
    public void DeveConsiderarSomenteAQuantidadeSolicitada()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 3, 4, 5,
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseFrequenciaRecente();

        var resultado = analise.Calcular(concursos, 1);

        Assert.Equal(1, resultado[1]);
        Assert.Equal(1, resultado[10]);
        Assert.Equal(0, resultado[16]);
        Assert.Equal(1, resultado[25]);
    }

    [Fact]
    public void DeveOrdenarConcursosAntesDeCalcular()
    {
        var concursos = new[]
        {
            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    21, 22, 23, 24, 25
                ]),

            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 3, 4, 5,
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseFrequenciaRecente();

        var resultado = analise.Calcular(concursos, 1);

        Assert.Equal(1, resultado[6]);
        Assert.Equal(0, resultado[16]);
    }

    [Fact]
    public void DeveRejeitarQuantidadeDeConcursosInvalida()
    {
        var analise = new AnaliseFrequenciaRecente();

        var excecao = Assert.Throws<ArgumentException>(
            () => analise.Calcular([], 0));

        Assert.Equal(
            "quantidadeConcursos",
            excecao.ParamName);
    }

    [Fact]
    public void DeveConsiderarTodosOsConcursosQuandoQuantidadeSolicitadaForMaiorQueOHistorico()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 3, 4, 5,
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseFrequenciaRecente();

        var resultado = analise.Calcular(concursos, 10);

        Assert.Equal(2, resultado[1]);
        Assert.Equal(1, resultado[15]);
        Assert.Equal(1, resultado[25]);

        Assert.Equal(30, resultado.Values.Sum());
    }
}