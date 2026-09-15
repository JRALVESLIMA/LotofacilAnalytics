using LotofacilAnalytics.Analysis.Faixas;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Faixas;

public class AnaliseDistribuicaoFaixasTests
{
    [Fact]
    public void DeveDistribuirDezenasNasFaixasCorretamente()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 3, 5,
                6, 8, 10,
                11, 13, 15,
                16, 18, 20,
                21, 23, 25
            ]);

        var analise = new AnaliseDistribuicaoFaixas();

        var resultado = analise.Calcular(concurso);

        Assert.Equal(3, resultado["01-05"]);
        Assert.Equal(3, resultado["06-10"]);
        Assert.Equal(3, resultado["11-15"]);
        Assert.Equal(3, resultado["16-20"]);
        Assert.Equal(3, resultado["21-25"]);
    }

    [Fact]
    public void DeveGarantirQueTodasAsDezenasForamDistribuidas()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10,
                11, 12, 13, 14, 15
            ]);

        var analise = new AnaliseDistribuicaoFaixas();

        var resultado = analise.Calcular(concurso);

        var total = resultado.Values.Sum();

        Assert.Equal(15, total);
    }

    [Fact]
    public void DeveRegistrarZeroQuandoFaixaNaoPossuirDezenas()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10,
                11, 12, 13, 14, 15
            ]);

        var analise = new AnaliseDistribuicaoFaixas();

        var resultado = analise.Calcular(concurso);

        Assert.Equal(5, resultado["01-05"]);
        Assert.Equal(5, resultado["06-10"]);
        Assert.Equal(5, resultado["11-15"]);
        Assert.Equal(0, resultado["16-20"]);
        Assert.Equal(0, resultado["21-25"]);
    }
}