using LotofacilAnalytics.Analysis.Sequencias;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Sequencias;

public class AnaliseParesConsecutivosTests
{
    [Fact]
    public void DeveCalcularQuantidadeDeParesConsecutivos()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3,
                5, 6,
                10,
                12, 13,
                15,
                18,
                20, 21, 22, 23,
                25
            ]);

        var analise = new AnaliseParesConsecutivos();

        var resultado = analise.Calcular([concurso]);

        // 1-2, 2-3 = 2
        // 5-6 = 1
        // 12-13 = 1
        // 20-21, 21-22, 22-23 = 3
        //
        // Total = 7
        Assert.Equal(1, resultado[7]);
    }

    [Fact]
    public void DeveCalcularQuantidadeDeParesConsecutivosEmUmConcursoValido()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 3, 5, 7, 9,
                11, 13, 15, 17, 19,
                21, 22, 23, 24, 25
            ]);

        var analise = new AnaliseParesConsecutivos();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(1, resultado[4]);
    }

    [Fact]
    public void DeveAcumularConcursosComAMesmaQuantidadeDePares()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 4, 6, 8,
                    10, 12, 14, 16, 18,
                    20, 22, 23, 24, 25
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 4, 6, 8,
                    10, 12, 14, 16, 18,
                    20, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseParesConsecutivos();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado[4]);
    }

    [Fact]
    public void DeveOrdenarAsDezenasAntesDeCalcular()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                5, 3, 4, 2, 1,
                10, 8, 9, 7, 6,
                15, 13, 14, 12, 11
            ]);

        var analise = new AnaliseParesConsecutivos();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(1, resultado[14]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseParesConsecutivos();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveRegistrarZeroParesQuandoNaoHouverDezenasConsecutivas()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 3, 5, 7, 9,
                11, 13, 15, 17, 19,
                21, 22, 23, 24, 25
            ]);

        var analise = new AnaliseParesConsecutivos();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(1, resultado[4]);
    }
}