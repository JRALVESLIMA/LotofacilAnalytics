using LotofacilAnalytics.Analysis.Sequencias;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Sequencias;

public class AnaliseSequenciasDezenasTests
{
    [Fact]
    public void DeveIdentificarMaiorSequencia()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 7, 8,
                10, 11, 12,
                18,
                20, 21, 22, 23, 24, 25
            ]);

        var analise = new AnaliseSequenciasDezenas();

        var resultado = analise.CalcularMaiorSequencia(concurso);

        Assert.Equal(6, resultado);
    }

    [Fact]
    public void DeveIdentificarSequenciaDeTamanhoTres()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3,
                5, 7, 9, 11,
                13, 15, 17, 19,
                21, 22, 24, 25
            ]);

        var analise = new AnaliseSequenciasDezenas();

        var resultado = analise.CalcularMaiorSequencia(concurso);

        Assert.Equal(3, resultado);
    }

    [Fact]
    public void DeveIdentificarMaiorSequenciaQuandoExistemOutrasDezenasConsecutivas()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3,
                5, 6,
                8, 9,
                11, 12,
                14, 15,
                17, 18,
                20, 23
            ]);

        var analise = new AnaliseSequenciasDezenas();

        var resultado = analise.CalcularMaiorSequencia(concurso);

        Assert.Equal(3, resultado);
    }

    [Fact]
    public void DeveOrdenarAsDezenasAntesDeCalcular()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                5, 4, 3, 2, 1,
                10, 12, 14, 16, 18,
                20, 22, 23, 24, 25
            ]);

        var analise = new AnaliseSequenciasDezenas();

        var resultado = analise.CalcularMaiorSequencia(concurso);

        Assert.Equal(5, resultado);
    }

    [Fact]
    public void DeveIdentificarSequenciaComTodasAsQuinzeDezenas()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10,
                11, 12, 13, 14, 15
            ]);

        var analise = new AnaliseSequenciasDezenas();

        var resultado = analise.CalcularMaiorSequencia(concurso);

        Assert.Equal(15, resultado);
    }

    [Fact]
    public void DeveCalcularCadaConcursoSeparadamente()
    {
        var concurso1 = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 4,
                7, 9, 11, 13, 15,
                17, 19, 21, 23, 24, 25
            ]);

        var concurso2 = new Concurso(
            2,
            new DateTime(2026, 1, 2),
            [
                1, 2, 3, 4, 5,
                8, 10, 12, 14, 16,
                18, 20, 22, 24, 25
            ]);

        var analise = new AnaliseSequenciasDezenas();

        var resultado =
            analise.Calcular([concurso1, concurso2]);

        Assert.Equal(4, resultado[1]);
        Assert.Equal(5, resultado[2]);
    }

    [Fact]
    public void DeveRetornarDicionarioVazioQuandoNaoExistemConcursos()
    {
        var analise = new AnaliseSequenciasDezenas();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoConcursoForNulo()
    {
        var analise = new AnaliseSequenciasDezenas();

        Assert.Throws<ArgumentNullException>(() =>
            analise.CalcularMaiorSequencia(null!));
    }
}