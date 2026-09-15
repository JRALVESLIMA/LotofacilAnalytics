using LotofacilAnalytics.Analysis.Atrasos;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Atrasos;

public class AnaliseMaiorAtrasoDezenasTests
{
    [Fact]
    public void DeveCalcularMaiorAtrasoDasDezenas()
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
                ]),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    21, 22, 23, 24, 25
                ]),

            new Concurso(
                4,
                new DateTime(2026, 1, 4),
                [
                    1, 2, 3, 4, 5,
                    11, 12, 13, 14, 15,
                    21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseMaiorAtrasoDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(0, resultado[1]);
        Assert.Equal(1, resultado[6]);
        Assert.Equal(0, resultado[16]);
        Assert.Equal(0, resultado[20]);
    }

    [Fact]
    public void DeveOrdenarConcursosAntesDeCalcular()
    {
        var concursos = new[]
        {
            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    21, 22, 23, 24, 25
                ]),

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

        var analise = new AnaliseMaiorAtrasoDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(0, resultado[16]);
        Assert.Equal(0, resultado[20]);
    }

    [Fact]
    public void DeveRetornarZeroQuandoDezenaApareceEmTodosOsConcursos()
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
                    16, 17, 18, 19, 20
                ])
        };

        var analise = new AnaliseMaiorAtrasoDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(0, resultado[1]);
    }

    [Fact]
    public void DeveRetornarResultadoComTodasAsDezenas()
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

        var analise = new AnaliseMaiorAtrasoDezenas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(25, resultado.Count);
        Assert.True(resultado.ContainsKey(25));
    }

    [Fact]
    public void DeveRetornarTodasAsDezenasComAtrasoZeroQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseMaiorAtrasoDezenas();

        var resultado = analise.Calcular([]);

        Assert.Equal(25, resultado.Count);

        for (var dezena = 1; dezena <= 25; dezena++)
        {
            Assert.True(resultado.ContainsKey(dezena));
            Assert.Equal(0, resultado[dezena]);
        }
    }
}