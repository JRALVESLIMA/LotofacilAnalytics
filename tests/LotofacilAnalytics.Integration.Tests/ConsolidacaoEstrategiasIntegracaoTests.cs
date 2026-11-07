
using FluentAssertions;
using LotofacilAnalytics.App.Services;

namespace LotofacilAnalytics.Integration.Tests;

public class ConsolidacaoEstrategiasIntegracaoTests
{
    [Fact]
    public async Task Deve_consolidar_estrategias_com_historico_real()
    {
        // Arrange
        var diretorioRaw = EncontrarDiretorioRaw();

        var aplicacaoService = new AplicacaoService(diretorioRaw);

        var historico = await aplicacaoService.CarregarHistoricoAsync();

        historico.Should().NotBeEmpty();

        // Act
        var resultadosPeriodos =
            aplicacaoService.CompararEstrategiasPorPeriodos(historico);

        var consolidados =
            aplicacaoService.ConsolidarEstrategias(resultadosPeriodos);

        // Assert
        resultadosPeriodos.Should().HaveCount(16);

        consolidados.Should().HaveCount(4);

        consolidados
            .Select(resultado => resultado.NomeEstrategia)
            .Should()
            .BeEquivalentTo(new[]
            {
                "Frequencia Geral",
                "Frequencia Recente",
                "Dezenas Fixas",
                "Aleatoria"
            });

        foreach (var consolidado in consolidados)
        {
            consolidado.QuantidadePeriodos.Should().Be(4);
            consolidado.QuantidadeJogos.Should().Be(400);

            consolidado.MediaAcertos.Should().BeInRange(0, 15);
            consolidado.MedianaAcertos.Should().BeInRange(0, 15);

            consolidado.DesvioPadraoAcertos
                .Should().BeGreaterThanOrEqualTo(0);

            consolidado.PercentualJogosCom11OuMaisAcertos
                .Should().BeInRange(0, 100);

            var concursos = consolidado.Resultado.Rodadas
                .Select(rodada => rodada.ConcursoAlvo.Numero)
                .ToArray();

            concursos.Should().HaveCount(400);
            concursos.Should().OnlyHaveUniqueItems();

            var resultadosOriginais = resultadosPeriodos
                .Where(resultado =>
                    resultado.NomeEstrategia == consolidado.NomeEstrategia)
                .ToArray();

            var quantidadeJogosOriginais = resultadosOriginais
                .Sum(resultado => resultado.QuantidadeJogos);

            consolidado.QuantidadeJogos
                .Should().Be(quantidadeJogosOriginais);

            var acertosOriginais = resultadosOriginais
                .SelectMany(resultado => resultado.Resultado.Rodadas)
                .SelectMany(rodada => rodada.ResultadosJogos)
                .Select(jogo => jogo.QuantidadeAcertos)
                .OrderBy(acertos => acertos)
                .ToArray();

            var acertosConsolidados = consolidado.Resultado.Rodadas
                .SelectMany(rodada => rodada.ResultadosJogos)
                .Select(jogo => jogo.QuantidadeAcertos)
                .OrderBy(acertos => acertos)
                .ToArray();

            acertosConsolidados.Should().Equal(acertosOriginais);
        }

        consolidados.Sum(resultado => resultado.QuantidadeJogos)
            .Should().Be(1600);
    }

    private static string EncontrarDiretorioRaw()
    {
        var diretorio = new DirectoryInfo(
            AppContext.BaseDirectory);

        while (diretorio is not null)
        {
            var diretorioRaw = Path.Combine(
                diretorio.FullName,
                "data",
                "raw");

            if (Directory.Exists(diretorioRaw))
                return diretorioRaw;

            diretorio = diretorio.Parent;
        }

        throw new DirectoryNotFoundException(
            "Não foi possível localizar o diretório data/raw.");
    }
}