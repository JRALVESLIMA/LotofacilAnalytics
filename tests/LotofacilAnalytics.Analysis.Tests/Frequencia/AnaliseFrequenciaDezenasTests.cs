using LotofacilAnalytics.Analysis.Frequencia;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Frequencia;

public class AnaliseFrequenciaDezenasTests
{
    [Fact]
    public void DeveCalcularFrequenciaDasDezenas()
    {
        var concurso1 = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10,
                11, 12, 13, 14, 15
            ]);

        var concurso2 = new Concurso(
            2,
            new DateTime(2026, 1, 2),
            [
                1, 2, 3, 4, 5,
                16, 17, 18, 19, 20,
                21, 22, 23, 24, 25
            ]);

        var concursos = new[]
        {
            concurso1,
            concurso2
        };

        var analise = new AnaliseFrequenciaDezenas();

        var resultado = analise.CalcularFrequencia(concursos);

        Assert.Equal(2, resultado[1]);
        Assert.Equal(2, resultado[5]);

        Assert.Equal(1, resultado[15]);
        Assert.Equal(1, resultado[25]);

        Assert.Equal(25, resultado.Count);

        Assert.Equal(30, resultado.Values.Sum());
    }

    [Fact]
    public void DeveRetornarFrequenciaZeroQuandoNaoHouverConcursos()
    {
        var concursos = Array.Empty<Concurso>();

        var analise = new AnaliseFrequenciaDezenas();

        var resultado = analise.CalcularFrequencia(concursos);

        Assert.Equal(25, resultado.Count);

        Assert.All(
            resultado.Values,
            frequencia => Assert.Equal(0, frequencia));
    }
}