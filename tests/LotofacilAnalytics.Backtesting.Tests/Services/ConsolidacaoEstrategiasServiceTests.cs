
using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Domain.Entities;
using Xunit;

namespace LotofacilAnalytics.Backtesting.Tests.Services;

public class ConsolidacaoEstrategiasServiceTests
{
    private readonly ConsolidacaoEstrategiasService _service = new();

    [Fact]
    public void Deve_agrupar_periodos_por_estrategia()
    {
        var resultados = new[]
        {
            CriarResultado("Aleatoria", 2, 3, 8, 10),
            CriarResultado("Frequencia", 2, 3, 9, 11),
            CriarResultado("Aleatoria", 4, 5, 10, 12),
            CriarResultado("Frequencia", 4, 5, 8, 10)
        };

        var consolidados = _service.Consolidar(resultados);

        Assert.Equal(2, consolidados.Count);

        Assert.All(consolidados, resultado =>
        {
            Assert.Equal(2, resultado.QuantidadePeriodos);
            Assert.Equal(4, resultado.QuantidadeJogos);
            Assert.Equal(4, resultado.Resultado.QuantidadeRodadas);
        });
    }

    [Fact]
    public void Deve_calcular_indicadores_sobre_todos_os_jogos()
    {
        var resultados = new[]
        {
            CriarResultado("Aleatoria", 2, 3, 8, 10),
            CriarResultado("Aleatoria", 4, 5, 10, 12)
        };

        var consolidado = Assert.Single(
            _service.Consolidar(resultados));

        Assert.Equal(4, consolidado.QuantidadeJogos);
        Assert.Equal(10, consolidado.MediaAcertos);
        Assert.Equal(10, consolidado.MedianaAcertos);
        Assert.Equal(2, consolidado.VarianciaAcertos, 10);
        Assert.Equal(
            Math.Sqrt(2),
            consolidado.DesvioPadraoAcertos,
            10);
        Assert.Equal(
            25,
            consolidado.PercentualJogosCom11OuMaisAcertos);
    }

    [Fact]
    public void Deve_retornar_colecao_vazia_sem_resultados()
    {
        var consolidados = _service.Consolidar(
            Array.Empty<ComparacaoPeriodoResultado>());

        Assert.Empty(consolidados);
    }

    [Fact]
    public void Deve_rejeitar_resultados_nulos()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _service.Consolidar(null!));
    }

    [Fact]
    public void Deve_rejeitar_periodos_sobrepostos_da_mesma_estrategia()
    {
        var resultados = new[]
        {
            CriarResultado("Aleatoria", 2, 4, 8, 9, 10),
            CriarResultado("Aleatoria", 4, 6, 9, 10, 11)
        };

        Assert.Throws<ArgumentException>(() =>
            _service.Consolidar(resultados));
    }

    [Fact]
    public void Deve_rejeitar_concursos_duplicados_na_mesma_estrategia()
    {
        var primeiroResultado = CriarResultado(
            "Aleatoria", 100, 199, 8, 9);

        var segundoResultado = CriarResultado(
            "Aleatoria", 200, 299, 10, 11);

        var rodadaDuplicada = primeiroResultado
            .Resultado.Rodadas.First();

        var rodadasSegundoPeriodo = segundoResultado
            .Resultado.Rodadas
            .Append(rodadaDuplicada)
            .ToArray();

        var segundoResultadoComDuplicidade =
            new ComparacaoPeriodoResultado(
                200,
                299,
                "Aleatoria",
                new BacktestingResultado(rodadasSegundoPeriodo));

        var resultados = new[]
        {
            primeiroResultado,
            segundoResultadoComDuplicidade
        };

        Assert.Throws<ArgumentException>(() =>
            _service.Consolidar(resultados));
    }

    private static ComparacaoPeriodoResultado CriarResultado(
        string nomeEstrategia,
        int inicio,
        int fim,
        params int[] acertos)
    {
        var rodadas = acertos
            .Select((quantidadeAcertos, indice) =>
            {
                var concurso = new Concurso(
                    inicio + indice,
                    new DateTime(2026, 1, 1).AddDays(indice),
                    Enumerable.Range(1, 15));

                var jogo = new ResultadoJogo(
                    Enumerable.Range(1, 15),
                    quantidadeAcertos);

                return new RodadaBacktesting(
                    concurso,
                    Array.Empty<Concurso>(),
                    new[] { jogo });
            })
            .ToArray();

        return new ComparacaoPeriodoResultado(
            inicio,
            fim,
            nomeEstrategia,
            new BacktestingResultado(rodadas));
    }
}