using FluentAssertions;
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.DataCollector.Mappers;
using LotofacilAnalytics.DataCollector.Storage;
using LotofacilAnalytics.DataCollector.Validation;
using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.Backtesting.Models;

namespace LotofacilAnalytics.Integration.Tests;

public class BacktestingIntegracaoTests
{
    [Fact]
    public async Task Deve_carregar_historico_real_e_executar_backtesting()
    {
        var diretorioRaw = EncontrarDiretorioRaw();

        var reader = CriarReader(diretorioRaw);

        var historico = await reader.LerTodosAsync();

        historico.Should().NotBeEmpty();

        ValidarIntegridadeHistorico(historico);

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        var rodadas = service.Executar(
            historico,
            estrategia);

        
        var resultado = new BacktestingResultado(rodadas);

        resultado.QuantidadeRodadas
            .Should()
            .Be(historico.Count - 1);

        resultado.QuantidadeJogos
            .Should()
            .Be(historico.Count - 1);

        resultado.MediaAcertos
            .Should()
            .BeGreaterThanOrEqualTo(0);

        resultado.MediaAcertos
            .Should()
            .BeLessThanOrEqualTo(15);

        resultado.MaiorAcertos
            .Should()
            .BeGreaterThanOrEqualTo(resultado.MenorAcertos);


        var primeiroConcurso = historico
            .OrderBy(concurso => concurso.Numero)
            .First();

        var ultimoConcurso = historico
            .OrderBy(concurso => concurso.Numero)
            .Last();

        rodadas.Should().HaveCount(historico.Count - 1);
        rodadas.First().ConcursoAlvo.Numero
            .Should().Be(primeiroConcurso.Numero + 1);

        rodadas.Last().ConcursoAlvo.Numero
            .Should().Be(ultimoConcurso.Numero);
    }

    [Fact]
    public async Task Deve_executar_estrategia_de_frequencia_com_historico_real()
    {
        var diretorioRaw = EncontrarDiretorioRaw();
        var reader = CriarReader(diretorioRaw);

        var historico = await reader.LerTodosAsync();

        historico.Should().NotBeEmpty();
        ValidarIntegridadeHistorico(historico);

        var estrategia = new EstrategiaFrequenciaDezenas();
        var service = new BacktestingService();

        var resultado = service.ExecutarConsolidado(
            historico,
            estrategia);

        resultado.QuantidadeRodadas
            .Should()
            .Be(historico.Count - 1);

        resultado.QuantidadeJogos
            .Should()
            .Be(resultado.QuantidadeRodadas);

        resultado.Rodadas.Should().OnlyContain(
            rodada => rodada.ResultadosJogos.Count == 1);

        resultado.Rodadas.Should().OnlyContain(
            rodada => rodada.ResultadosJogos
                .Single()
                .QuantidadeAcertos >= 0
                    && rodada.ResultadosJogos
                        .Single()
                        .QuantidadeAcertos <= 15);

        resultado.ObterDistribuicaoAcertos()
            .Values
            .Sum()
            .Should()
            .Be(resultado.QuantidadeJogos);
    }


    private static ConcursoHistoricoReader CriarReader(
        string diretorioRaw)
    {
        return new ConcursoHistoricoReader(
            diretorioRaw,
            new ConcursoValidator(),
            new ConcursoMapper());
    }

    private static void ValidarIntegridadeHistorico(
        IReadOnlyCollection<Concurso> historico)
    {
        var concursosOrdenados = historico
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        concursosOrdenados
            .Select(concurso => concurso.Numero)
            .Should()
            .OnlyHaveUniqueItems();

        concursosOrdenados
            .First()
            .Numero
            .Should()
            .Be(1);

        for (var i = 1; i < concursosOrdenados.Count; i++)
        {
            concursosOrdenados[i].Numero
                .Should()
                .Be(concursosOrdenados[i - 1].Numero + 1);
        }
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

    private sealed class EstrategiaFake : IEstrategia
    {
        public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
            IReadOnlyCollection<Concurso> historico)
        {
            return new[]
            {
                (IReadOnlyCollection<int>)
                    Enumerable.Range(1, 15).ToArray()
            };
        }
    }
}