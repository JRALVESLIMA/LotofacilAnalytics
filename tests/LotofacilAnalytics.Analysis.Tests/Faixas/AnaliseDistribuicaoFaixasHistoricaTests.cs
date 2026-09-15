using LotofacilAnalytics.Analysis.Faixas;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Faixas;

public class AnaliseDistribuicaoFaixasHistoricaTests
{
    [Fact]
    public void DeveCalcularDistribuicaoHistoricaDasFaixas()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3,
                    6, 7, 8, 9,
                    11, 12,
                    16, 17, 18,
                    21, 22, 23
                ])
        };

        var analise = new AnaliseDistribuicaoFaixasHistorica();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado["01-05"][3]);
        Assert.Equal(1, resultado["06-10"][4]);
        Assert.Equal(1, resultado["11-15"][2]);
        Assert.Equal(1, resultado["16-20"][3]);
        Assert.Equal(1, resultado["21-25"][3]);
    }

    [Fact]
    public void DeveAcumularConcursosComAMesmaQuantidadeNaFaixa()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3,
                    6, 7, 8,
                    11, 12, 13,
                    16, 17, 18,
                    21, 22, 23
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 4,
                    6, 7, 9,
                    11, 12, 14,
                    16, 17, 19,
                    21, 22, 24
                ])
        };

        var analise = new AnaliseDistribuicaoFaixasHistorica();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado["01-05"][3]);
        Assert.Equal(2, resultado["06-10"][3]);
        Assert.Equal(2, resultado["11-15"][3]);
        Assert.Equal(2, resultado["16-20"][3]);
        Assert.Equal(2, resultado["21-25"][3]);
    }

    [Fact]
    public void DeveGarantirQueCadaConcursoPossuiQuinzeDezenasDistribuidas()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3,
                    6, 7, 8,
                    11, 12, 13,
                    16, 17, 18,
                    21, 22, 23
                ])
        };

        var analise = new AnaliseDistribuicaoFaixasHistorica();

        var resultado = analise.Calcular(concursos);

        var total =
            resultado.Sum(faixa =>
                faixa.Value.Sum(quantidade =>
                    quantidade.Key * quantidade.Value));

        Assert.Equal(15, total);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseDistribuicaoFaixasHistorica();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado["01-05"]);
        Assert.Empty(resultado["06-10"]);
        Assert.Empty(resultado["11-15"]);
        Assert.Empty(resultado["16-20"]);
        Assert.Empty(resultado["21-25"]);
    }

    [Fact]
    public void DeveRegistrarZeroQuandoUmaFaixaNaoPossuirDezenas()
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
                ])
        };

        var analise = new AnaliseDistribuicaoFaixasHistorica();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado["01-05"][5]);
        Assert.Equal(1, resultado["06-10"][5]);
        Assert.Equal(1, resultado["11-15"][5]);

        Assert.Equal(1, resultado["16-20"][0]);
        Assert.Equal(1, resultado["21-25"][0]);
    }
}