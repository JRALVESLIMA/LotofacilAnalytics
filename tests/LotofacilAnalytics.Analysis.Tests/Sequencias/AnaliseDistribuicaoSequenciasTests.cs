using LotofacilAnalytics.Analysis.Sequencias;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Sequencias;

public class AnaliseDistribuicaoSequenciasTests
{
    [Fact]
    public void DeveCalcularDistribuicaoDasMaioresSequencias()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3,
                    5, 7, 9, 11,
                    13, 15, 17, 19,
                    21, 23, 24, 25
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 4, 6, 8,
                    10, 11, 12, 14, 16,
                    18, 20, 22, 23, 24
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 3, 5, 7, 9,
                    11, 13, 15, 17, 19,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseDistribuicaoSequencias();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado[3]);
        Assert.Equal(1, resultado[5]);
    }

    [Fact]
    public void DeveAcumularConcursosComAMesmaMaiorSequencia()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3,
                    5, 7, 9, 11,
                    13, 15, 17, 19,
                    21, 23, 24, 25
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    2, 3, 4,
                    6, 8, 10, 12,
                    14, 16, 18, 20,
                    21, 23, 24, 25
                ])
        };

        var analise = new AnaliseDistribuicaoSequencias();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado[3]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseDistribuicaoSequencias();

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
                    1, 2, 3,
                    5, 7, 9, 11,
                    13, 15, 17, 19,
                    21, 23, 24, 25
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 4, 6, 8,
                    10, 11, 12, 14, 16,
                    18, 20, 22, 23, 24
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 3, 5, 7, 9,
                    11, 13, 15, 17, 19,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseDistribuicaoSequencias();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(3, resultado.Values.Sum());
    }
}