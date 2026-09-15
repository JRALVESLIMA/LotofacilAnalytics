using LotofacilAnalytics.Analysis.MinMax;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.MinMax;

public class AnaliseMenorMaiorDezenaTests
{
    [Fact]
    public void DeveCalcularDistribuicaoDasMenoresDezenas()
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
                3, 5, 6, 8, 9,
                10, 12, 13, 14, 16,
                17, 18, 20, 22, 25
            ]);

        var concurso3 = new Concurso(
            3,
            new DateTime(2026, 1, 3),
            [
                1, 4, 5, 7, 9,
                11, 12, 14, 15, 16,
                18, 20, 21, 23, 25
            ]);

        var analise = new AnaliseMenorMaiorDezena();

        var resultado = analise.CalcularMenores(
            [concurso1, concurso2, concurso3]);

        Assert.Equal(2, resultado[3]);
        Assert.Equal(1, resultado[1]);
    }

    [Fact]
    public void DeveCalcularDistribuicaoDasMaioresDezenas()
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
                3, 5, 6, 8, 9,
                10, 12, 13, 14, 16,
                17, 18, 20, 22, 25
            ]);

        var concurso3 = new Concurso(
            3,
            new DateTime(2026, 1, 3),
            [
                1, 4, 5, 7, 9,
                11, 12, 14, 15, 16,
                18, 20, 21, 23, 24
            ]);

        var analise = new AnaliseMenorMaiorDezena();

        var resultado = analise.CalcularMaiores(
            [concurso1, concurso2, concurso3]);

        Assert.Equal(2, resultado[25]);
        Assert.Equal(1, resultado[24]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseMenorMaiorDezena();

        var menores = analise.CalcularMenores([]);

        var maiores = analise.CalcularMaiores([]);

        Assert.Empty(menores);
        Assert.Empty(maiores);
    }
}
