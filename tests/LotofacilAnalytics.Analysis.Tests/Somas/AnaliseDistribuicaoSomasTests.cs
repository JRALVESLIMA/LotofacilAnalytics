using LotofacilAnalytics.Analysis.Somas;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Somas;

public class AnaliseDistribuicaoSomasTests
{
    [Fact]
    public void DeveCalcularDistribuicaoDasSomasPorFaixa()
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
                    3, 4, 5, 6, 7,
                    8, 9, 10, 11, 12,
                    13, 14, 15, 16, 17
                ])
        };

        var analise = new AnaliseDistribuicaoSomas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado["120-129"]);
        Assert.Equal(1, resultado["150-159"]);
    }

    [Fact]
    public void DeveAcumularConcursosNaMesmaFaixa()
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

        var analise = new AnaliseDistribuicaoSomas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado["120-129"]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseDistribuicaoSomas();

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
                    3, 4, 5, 6, 7,
                    8, 9, 10, 11, 12,
                    13, 14, 15, 16, 17
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    5, 6, 7, 8, 9,
                    10, 11, 12, 13, 14,
                    15, 16, 17, 18, 19
                ])
        };

        var analise = new AnaliseDistribuicaoSomas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(3, resultado.Values.Sum());
    }
}