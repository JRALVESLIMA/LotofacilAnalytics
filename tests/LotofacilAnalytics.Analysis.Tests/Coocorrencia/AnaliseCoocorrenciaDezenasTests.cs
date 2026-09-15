using LotofacilAnalytics.Analysis.Coocorrencia;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Coocorrencia;

public class AnaliseCoocorrenciaDezenasTests
{
    [Fact]
    public void DeveCalcularCoocorrenciaDosPares()
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

        var analise = new AnaliseCoocorrenciaDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado[(1, 2)]);
        Assert.Equal(1, resultado[(1, 15)]);
        Assert.Equal(1, resultado[(7, 10)]);

        Assert.Equal(105, resultado.Count);
    }

    [Fact]
    public void DeveAcumularParesQueAparecemEmMaisDeUmConcurso()
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
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseCoocorrenciaDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado[(1, 2)]);
        Assert.Equal(2, resultado[(1, 5)]);

        Assert.Equal(1, resultado[(1, 16)]);
        Assert.Equal(1, resultado[(20, 25)]);
    }

    [Fact]
    public void DeveManterAsDezenasDoParEmOrdemCrescente()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    5, 4, 3, 2, 1,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ])
        };

        var analise = new AnaliseCoocorrenciaDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.True(resultado.ContainsKey((1, 5)));
        Assert.False(resultado.ContainsKey((5, 1)));
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseCoocorrenciaDezenas();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void NaoDeveCriarParComADezenaComElaMesma()
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

        var analise = new AnaliseCoocorrenciaDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.DoesNotContain(
            resultado.Keys,
            par => par.Dezena1 == par.Dezena2);
    }
}