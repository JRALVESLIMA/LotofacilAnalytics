using LotofacilAnalytics.Analysis.Services;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Services;

public class AnaliseEstatisticaServiceTests
{
    private static readonly int[] DezenasValidas =
    [
        3, 4, 5, 7, 8,
        10, 11, 13, 14, 16,
        17, 19, 23, 24, 25
    ];

    [Fact]
    public void DeveCalcularFrequenciaAtravésDoServico()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                DezenasValidas),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                DezenasValidas)
        };

        var service = new AnaliseEstatisticaService();

        var resultado = service.CalcularFrequencia(concursos);

        Assert.Equal(2, resultado[3]);
        Assert.Equal(2, resultado[25]);
        Assert.Equal(25, resultado.Count);
    }

    [Fact]
    public void DeveCalcularAtrasoAtravésDoServico()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                DezenasValidas),

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

        var service = new AnaliseEstatisticaService();

        var resultado = service.CalcularAtrasos(concursos);

        Assert.Equal(0, resultado[1]);
        Assert.Equal(2, resultado[16]);
        Assert.Equal(2, resultado[25]);
    }

    [Fact]
    public void DeveCalcularCoocorrenciaAtravésDoServico()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10,
                11, 12, 13, 14, 15
            ]);

        var service = new AnaliseEstatisticaService();

        var resultado = service.CalcularCoocorrencia([concurso]);

        Assert.Equal(1, resultado[(1, 2)]);
        Assert.Equal(1, resultado[(1, 15)]);
        Assert.Equal(1, resultado[(7, 10)]);
        Assert.Equal(105, resultado.Count);
    }

    [Fact]
    public void DeveCalcularIntervalosAtravésDoServico()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 4, 7, 8,
                10, 13, 14, 18, 19,
                20, 22, 23, 24, 25
            ]);

        var service = new AnaliseEstatisticaService();

        var resultado = service.CalcularIntervalos([concurso]);

        Assert.Equal(8, resultado[1]);
        Assert.Equal(3, resultado[2]);
        Assert.Equal(2, resultado[3]);
        Assert.Equal(1, resultado[4]);
    }

    [Fact]
    public void DeveRetornarTodasAsDezenasComFrequenciaZeroQuandoHistoricoEstiverVazio()
    {
        var service = new AnaliseEstatisticaService();

        var resultado = service.CalcularFrequencia([]);

        Assert.Equal(25, resultado.Count);

        foreach (var dezena in resultado)
        {
            Assert.Equal(0, dezena.Value);
        }
    }
}
