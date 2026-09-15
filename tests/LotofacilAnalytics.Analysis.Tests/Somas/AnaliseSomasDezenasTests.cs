using LotofacilAnalytics.Analysis.Somas;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Somas;

public class AnaliseSomasDezenasTests
{
    [Fact]
    public void DeveCalcularDistribuicaoDasSomas()
    {
        var concurso1 = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                3, 4, 5, 7, 8,
                10, 11, 13, 14, 16,
                17, 19, 23, 24, 25
            ]);

        var concurso2 = new Concurso(
            2,
            new DateTime(2026, 1, 2),
            [
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10,
                11, 12, 13, 14, 15
            ]);

        var analise = new AnaliseSomasDezenas();

        var resultado = analise.Calcular(
            [concurso1, concurso2]);

        Assert.Equal(1, resultado[199]);
        Assert.Equal(1, resultado[120]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseSomasDezenas();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveGarantirQueCadaConcursoContribuiUmaVezParaADistribuicao()
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
                    2, 3, 4, 5, 6,
                    7, 8, 9, 10, 11,
                    12, 13, 14, 15, 16
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    3, 4, 5, 6, 7,
                    8, 9, 10, 11, 12,
                    13, 14, 15, 16, 17
                ])
        };

        var analise = new AnaliseSomasDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(3, resultado.Values.Sum());
    }
}