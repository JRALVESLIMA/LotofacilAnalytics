using LotofacilAnalytics.Analysis.Atrasos;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Atrasos;

public class AnaliseAtrasoDezenasTests
{
    [Fact]
    public void DeveCalcularAtrasoDasDezenas()
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

        var analise = new AnaliseAtrasoDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(0, resultado[1]);
        Assert.Equal(0, resultado[10]);
        Assert.Equal(1, resultado[16]);
        Assert.Equal(1, resultado[20]);
        Assert.Equal(0, resultado[25]);
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

        var analise = new AnaliseAtrasoDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado[16]);
        Assert.Equal(1, resultado[20]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseAtrasoDezenas();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveConsiderarAtrasoDoUltimoConcursoParaDezenaNuncaSorteada()
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
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ])
        };

        var analise = new AnaliseAtrasoDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(3, resultado[16]);
        Assert.Equal(3, resultado[25]);
    }
}