using LotofacilAnalytics.Analysis.Faixas;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Tests.Faixas;

public class AnaliseBaixasAltasTests
{
    [Fact]
    public void DeveCalcularDistribuicaoEntreBaixasEAltas()
    {
        var concurso = new Concurso(
            1,
            new DateTime(2026, 1, 1),
            [
                1, 2, 3, 4, 5,
                6, 7, 8,
                14, 15, 16, 17, 18, 19, 20
            ]);

        var analise = new AnaliseBaixasAltas();

        var resultado = analise.Calcular([concurso]);

        Assert.Equal(1, resultado["8 baixas / 7 altas"]);
    }

    [Fact]
    public void DeveAcumularConcursosComAMesmaDistribuicao()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8,
                    14, 15, 16, 17, 18, 19, 20
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 4, 5,
                    6, 7, 8, 9,
                    14, 15, 17, 18, 19, 20, 21
                ])
        };

        var analise = new AnaliseBaixasAltas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(2, resultado["8 baixas / 7 altas"]);
    }

    [Fact]
    public void DeveGarantirQueBaixasEAltasSomamQuinze()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8,
                    14, 15, 16, 17, 18, 19, 20
                ])
        };

        var analise = new AnaliseBaixasAltas();

        var resultado = analise.Calcular(concursos);

        var total = resultado
            .Sum(item =>
            {
                var partes = item.Key.Split(" / ");

                var baixas = int.Parse(
                    partes[0].Replace(" baixas", ""));

                var altas = int.Parse(
                    partes[1].Replace(" altas", ""));

                return (baixas + altas) * item.Value;
            });

        Assert.Equal(15, total);
    }

    [Fact]
    public void DeveRetornarResultadoVazioQuandoNaoHouverConcursos()
    {
        var analise = new AnaliseBaixasAltas();

        var resultado = analise.Calcular([]);

        Assert.Empty(resultado);
    }

    [Fact]
    public void DeveCalcularDistribuicoesNosLimitesPossiveis()
    {
        var concursos = new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                [
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13,
                    14, 15
                ]),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                [
                    1, 2, 3,
                    14, 15, 16, 17, 18, 19,
                    20, 21, 22, 23, 24, 25
                ])
        };

        var analise = new AnaliseBaixasAltas();

        var resultado = analise.Calcular(concursos);

        Assert.Equal(1, resultado["13 baixas / 2 altas"]);
        Assert.Equal(1, resultado["3 baixas / 12 altas"]);
    }
}