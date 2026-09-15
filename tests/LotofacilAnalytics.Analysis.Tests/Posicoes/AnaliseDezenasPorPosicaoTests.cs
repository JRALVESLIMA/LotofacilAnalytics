using LotofacilAnalytics.Analysis.Posicoes;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Posicoes;

public class AnaliseDezenasPorPosicaoTests
{
    [Fact]
    public void DeveCalcularDezenasPorPosicao()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 3, 5, 7, 9,
                11, 13, 15, 17, 19,
                21, 22, 23, 24, 25
            ]);

        var analise = new AnaliseDezenasPorPosicao();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(1, resultado[1][1]);
        Assert.Equal(1, resultado[2][3]);
        Assert.Equal(1, resultado[3][5]);
        Assert.Equal(1, resultado[15][25]);
    }

    [Fact]
    public void DeveAcumularOcorrenciasDaMesmaDezenaNaMesmaPosicao()
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

        var analise = new AnaliseDezenasPorPosicao();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado[1][1]);
        Assert.Equal(2, resultado[5][5]);
        Assert.Equal(2, resultado[15][15]);
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

        var analise = new AnaliseDezenasPorPosicao();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(1, resultado[1][1]);
        Assert.Equal(1, resultado[5][5]);
        Assert.Equal(1, resultado[15][15]);
    }

    [Fact]
    public void DeveInicializarTodasAsPosicoesEDezenas()
    {
        var analise = new AnaliseDezenasPorPosicao();

        var resultado = analise.Calcular([]);

        Assert.Equal(15, resultado.Count);

        foreach (var posicao in resultado)
        {
            Assert.Equal(25, posicao.Value.Count);
            Assert.All(
                posicao.Value.Values,
                quantidade => Assert.Equal(0, quantidade));
        }
    }

    [Fact]
    public void DeveGarantirQueCadaPosicaoPossuiQuantidadeDeOcorrenciasIgualAoNumeroDeConcursos()
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
                    1, 3, 5, 7, 9,
                    11, 13, 15, 17, 19,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseDezenasPorPosicao();

        var resultado = analise.Calcular(concursos);

        foreach (var posicao in resultado)
        {
            Assert.Equal(2, posicao.Value.Values.Sum());
        }
    }
}