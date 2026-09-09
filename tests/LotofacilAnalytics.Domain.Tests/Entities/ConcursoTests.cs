using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Domain.Tests.Entities;

public class ConcursoTests
{
    private static readonly int[] DezenasValidas =
    [
        3, 4, 5, 7, 8,
        10, 11, 13, 14, 16,
        17, 19, 23, 24, 25
    ];

    [Fact]
    public void DeveCriarConcursoValido()
    {
        var concurso = new Concurso(
            3779,
            new DateTime(2026, 9, 3),
            DezenasValidas);

        Assert.Equal(3779, concurso.Numero);
        Assert.Equal(
            new DateTime(2026, 9, 3),
            concurso.DataSorteio);

        Assert.Equal(15, concurso.Dezenas.Count);
        Assert.Equal(DezenasValidas, concurso.Dezenas);
    }

    [Fact]
    public void DeveRejeitarNumeroDeConcursoInvalido()
    {
        Assert.Throws<ArgumentException>(() =>
            new Concurso(
                0,
                new DateTime(2026, 9, 3),
                DezenasValidas));
    }

    [Fact]
    public void DeveRejeitarDataInvalida()
    {
        Assert.Throws<ArgumentException>(() =>
            new Concurso(
                3779,
                default,
                DezenasValidas));
    }

    [Fact]
    public void DeveRejeitarQuantidadeDeDezenasDiferenteDe15()
    {
        var dezenas = new[]
        {
            1, 2, 3, 4, 5
        };

        Assert.Throws<ArgumentException>(() =>
            new Concurso(
                3779,
                new DateTime(2026, 9, 3),
                dezenas));
    }

    [Fact]
    public void DeveRejeitarDezenaMenorQue1()
    {
        var dezenas = DezenasValidas
            .Select((dezena, indice) =>
                indice == 0 ? 0 : dezena)
            .ToArray();

        Assert.Throws<ArgumentException>(() =>
            new Concurso(
                3779,
                new DateTime(2026, 9, 3),
                dezenas));
    }

    [Fact]
    public void DeveRejeitarDezenaMaiorQue25()
    {
        var dezenas = DezenasValidas
            .Select((dezena, indice) =>
                indice == 0 ? 26 : dezena)
            .ToArray();

        Assert.Throws<ArgumentException>(() =>
            new Concurso(
                3779,
                new DateTime(2026, 9, 3),
                dezenas));
    }

    [Fact]
    public void DeveRejeitarDezenasRepetidas()
    {
        var dezenas = DezenasValidas
            .Select((dezena, indice) =>
                indice == 1 ? DezenasValidas[0] : dezena)
            .ToArray();

        Assert.Throws<ArgumentException>(() =>
            new Concurso(
                3779,
                new DateTime(2026, 9, 3),
                dezenas));
    }

    [Fact]
    public void DeveRejeitarColecaoDeDezenasNula()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Concurso(
                3779,
                new DateTime(2026, 9, 3),
                null!));
    }
}