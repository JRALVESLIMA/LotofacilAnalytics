using LotofacilAnalytics.Analysis.ParesImpares;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.ParesImpares;

public class AnaliseParesImparesTests
{
    [Fact]
    public void DeveCalcularDistribuicaoDeParesEImpares()
    {
        var concurso1 = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                2, 4, 6, 8, 10,
                12, 14,
                1, 3, 5, 7, 9, 11, 13, 15
            ]);

        var concurso2 = new Concurso(
            2,
            new DateTime(2026, 1, 2),
            [
                2, 4, 6, 8, 10, 12, 14, 16,
                1, 3, 5, 7, 9, 11, 13
            ]);

        var concursos = new[]
        {
            concurso1,
            concurso2
        };

        var analise = new AnaliseParesImpares();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado["7 pares / 8 ímpares"]);
        Assert.Equal(1, resultado["8 pares / 7 ímpares"]);
    }

    [Fact]
    public void DeveAcumularConcursosComAMesmaDistribuicao()
    {
        var concurso1 = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                2, 4, 6, 8, 10, 12, 14,
                1, 3, 5, 7, 9, 11, 13, 15
            ]);

        var concurso2 = new Concurso(
            2,
            new DateTime(2026, 1, 2),
            [
                2, 4, 6, 8, 10, 12, 14,
                1, 3, 5, 7, 9, 11, 13, 15
            ]);

        var concurso3 = new Concurso(
            3,
            new DateTime(2026, 1, 3),
            [
                2, 4, 6, 8, 10, 12, 14,
                1, 3, 5, 7, 9, 11, 13, 15
            ]);

        var analise = new AnaliseParesImpares();

        var resultado = analise.Calcular(
            [concurso1, concurso2, concurso3]);

        Assert.Equal(3, resultado["7 pares / 8 ímpares"]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseParesImpares();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }
}