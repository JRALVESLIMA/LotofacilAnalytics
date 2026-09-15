using LotofacilAnalytics.Analysis.Repeticoes;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Repeticoes;

public class AnaliseRepeticaoConcursosTests
{
    [Fact]
    public void DeveCalcularQuantidadeDeDezenasRepetidas()
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

        var analise = new AnaliseRepeticaoConcursos();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado[5]);
    }

    [Fact]
    public void DeveAcumularConcursosComAMesmaQuantidadeDeRepeticoes()
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
                    11, 12, 13, 14, 15
                ])
        };

        var analise = new AnaliseRepeticaoConcursos();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado[5]);
    }

    [Fact]
    public void DeveOrdenarOsConcursosAntesDeCalcular()
    {
        var concursos = new[]
        {
            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                ]),

            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    11, 12, 13, 14, 15,
                    21, 22, 23, 24, 25
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

        var analise = new AnaliseRepeticaoConcursos();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado[5]);
        Assert.Equal(1, resultado[10]);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoHouverMenosDeDoisConcursos()
    {
        var analise = new AnaliseRepeticaoConcursos();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveRealizarUmaComparacaoParaCadaParDeConcursosConsecutivos()
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
                ])
        };

        var analise = new AnaliseRepeticaoConcursos();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado.Values.Sum());
    }
}