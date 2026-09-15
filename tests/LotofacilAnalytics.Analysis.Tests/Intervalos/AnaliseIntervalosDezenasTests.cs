using LotofacilAnalytics.Analysis.Intervalos;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Intervalos;

public class AnaliseIntervalosDezenasTests
{
    [Fact]
    public void DeveCalcularIntervalosEntreAsDezenas()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 4, 7, 8,
                10, 13, 14, 18, 19,
                20, 22, 23, 24, 25
            ]);

        var analise = new AnaliseIntervalosDezenas();

        var resultado = analise.Calcular([concurso]);

        // Intervalos:
        // 1-2  = 1
        // 2-4  = 2
        // 4-7  = 3
        // 7-8  = 1
        // 8-10 = 2
        // 10-13 = 3
        // 13-14 = 1
        // 14-18 = 4
        // 18-19 = 1
        // 19-20 = 1
        // 20-22 = 2
        // 22-23 = 1
        // 23-24 = 1
        // 24-25 = 1

        Assert.Equal(8, resultado[1]);
        Assert.Equal(3, resultado[2]);
        Assert.Equal(2, resultado[3]);
        Assert.Equal(1, resultado[4]);
    }

    [Fact]
    public void DeveAcumularIntervalosDeConcursosDiferentes()
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
                ])
        };

        var analise = new AnaliseIntervalosDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(28, resultado[1]);
    }

    [Fact]
    public void DeveOrdenarAsDezenasAntesDeCalcular()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                15, 14, 13, 12, 11,
                10, 9, 8, 7, 6,
                5, 4, 3, 2, 1
            ]);

        var analise = new AnaliseIntervalosDezenas();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(14, resultado[1]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseIntervalosDezenas();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveGarantirQueCadaConcursoPossuiQuatorzeIntervalos()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 4, 7, 8,
                10, 13, 14, 18, 19,
                20, 22, 23, 24, 25
            ]);

        var analise = new AnaliseIntervalosDezenas();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(14, resultado.Values.Sum());
    }
}