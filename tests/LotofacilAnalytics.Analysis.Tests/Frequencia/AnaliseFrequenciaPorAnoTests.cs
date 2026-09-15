using LotofacilAnalytics.Analysis.Frequencia;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Frequencia;

public class AnaliseFrequenciaPorAnoTests
{
    [Fact]
    public void DeveCalcularFrequenciaDasDezenasPorAno()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2025, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                2,
                new DateTime(2025, 6, 1),
                [
                    1, 2, 3, 4, 5,
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseFrequenciaPorAno();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado[2025][1]);
        Assert.Equal(1, resultado[2025][15]);
        Assert.Equal(1, resultado[2025][25]);

        Assert.Equal(1, resultado[2026][1]);
        Assert.Equal(1, resultado[2026][10]);
        Assert.Equal(1, resultado[2026][25]);
    }

    [Fact]
    public void DeveInicializarTodasAsDezenasParaCadaAno()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2025, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ])
        };

        var analise = new AnaliseFrequenciaPorAno();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(25, resultado[2025].Count);
        Assert.Equal(0, resultado[2025][25]);
    }

    [Fact]
    public void DeveGarantirQueCadaAnoPossuiQuantidadeCorretaDeOcorrencias()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2025, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                2,
                new DateTime(2025, 6, 1),
                [
                    1, 2, 3, 4, 5,
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseFrequenciaPorAno();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(30, resultado[2025].Values.Sum());
        Assert.Equal(15, resultado[2026].Values.Sum());
    }
}