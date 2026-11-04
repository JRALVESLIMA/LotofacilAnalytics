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

    [Fact]
    public async Task Deve_comparar_estrategias_no_mesmo_periodo_historico()
    {
        var diretorioRaw = EncontrarDiretorioRaw();
        var reader = CriarReader(diretorioRaw);
        var historico = await reader.LerTodosAsync();

        historico.Should().NotBeEmpty();
        ValidarIntegridadeHistorico(historico);

        var concursosOrdenados = historico
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        var numeroInicial = concursosOrdenados[^101].Numero;
        var numeroFinal = concursosOrdenados[^1].Numero;

        var estrategias = new Dictionary<string, IEstrategia>
        {
            ["Frequencia Geral"] = new EstrategiaFrequenciaDezenas(),
            ["Frequencia Recente"] = new EstrategiaFrequenciaRecente(50),
            ["Dezenas Fixas"] = new EstrategiaDezenasFixas(),
            ["Aleatoria"] = new EstrategiaAleatoria(semente: 12345)
        };

        var service = new ComparacaoEstrategiasService();

        var resultados = service.Comparar(
            historico,
            estrategias,
            numeroInicial,
            numeroFinal);

        resultados.Should().HaveCount(4);

        resultados
            .Select(resultado => resultado.NomeEstrategia)
            .Should()
            .Equal(
                "Frequencia Geral",
                "Frequencia Recente",
                "Dezenas Fixas",
                "Aleatoria");

        var concursosEsperados = concursosOrdenados
            .Where(concurso =>
                concurso.Numero >= numeroInicial &&
                concurso.Numero <= numeroFinal)
            .Select(concurso => concurso.Numero)
            .ToArray();

        concursosEsperados.Should().HaveCount(101);

        foreach (var resultado in resultados)
        {
            resultado.QuantidadeRodadas.Should().Be(101);
            resultado.QuantidadeJogos.Should().Be(101);

            resultado.Resultado.Rodadas
                .Select(rodada => rodada.ConcursoAlvo.Numero)
                .Should()
                .Equal(concursosEsperados);

            resultado.MediaAcertos.Should().BeInRange(0, 15);
            resultado.MaiorAcertos.Should().BeInRange(0, 15);
            resultado.MenorAcertos.Should().BeInRange(0, 15);

            resultado.MaiorAcertos
                .Should()
                .BeGreaterThanOrEqualTo(resultado.MenorAcertos);

            resultado.JogosCom11OuMaisAcertos
                .Should()
                .BeInRange(0, resultado.QuantidadeJogos);

            resultado.JogosCom13OuMaisAcertos
                .Should()
                .BeInRange(0, resultado.JogosCom11OuMaisAcertos);

            resultado.Resultado.ObterDistribuicaoAcertos()
                .Values.Sum()
                .Should()
                .Be(resultado.QuantidadeJogos);

            foreach (var rodada in resultado.Resultado.Rodadas)
            {
                rodada.ResultadosJogos.Should().ContainSingle();

                var jogo = rodada.ResultadosJogos.Single();

                jogo.Jogo.Should().HaveCount(15);
                jogo.Jogo.Should().OnlyHaveUniqueItems();
                jogo.Jogo.Should().OnlyContain(
                    dezena => dezena >= 1 && dezena <= 25);

                jogo.QuantidadeAcertos.Should().BeInRange(0, 15);
            }
        }
    }

    [Fact]
    public async Task Deve_executar_backtesting_em_dois_periodos_historicos_distintos()
    {
        var diretorioRaw = EncontrarDiretorioRaw();
        var reader = CriarReader(diretorioRaw);
        var historico = await reader.LerTodosAsync();

        historico.Should().NotBeEmpty();
        ValidarIntegridadeHistorico(historico);

        var concursosOrdenados = historico
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        var quantidadeConcursos = concursosOrdenados.Count;
        var indiceInicio = 1;
        var indiceMeio = quantidadeConcursos / 2;

        var primeiroPeriodoInicial = concursosOrdenados[indiceInicio].Numero;
        var primeiroPeriodoFinal = concursosOrdenados[indiceMeio - 1].Numero;

        var segundoPeriodoInicial = concursosOrdenados[indiceMeio].Numero;
        var segundoPeriodoFinal = concursosOrdenados[^1].Numero;

        var service = new BacktestingService();
        var estrategia = new EstrategiaFrequenciaDezenas();

        var primeiroResultado = service.ExecutarConsolidado(
            historico,
            estrategia,
            primeiroPeriodoInicial,
            primeiroPeriodoFinal);

        var segundoResultado = service.ExecutarConsolidado(
            historico,
            estrategia,
            segundoPeriodoInicial,
            segundoPeriodoFinal);

        primeiroResultado.Rodadas
            .Select(rodada => rodada.ConcursoAlvo.Numero)
            .Should()
            .Equal(
                concursosOrdenados
                    .Where(concurso =>
                        concurso.Numero >= primeiroPeriodoInicial &&
                        concurso.Numero <= primeiroPeriodoFinal)
                    .Select(concurso => concurso.Numero));

        segundoResultado.Rodadas
            .Select(rodada => rodada.ConcursoAlvo.Numero)
            .Should()
            .Equal(
                concursosOrdenados
                    .Where(concurso =>
                        concurso.Numero >= segundoPeriodoInicial &&
                        concurso.Numero <= segundoPeriodoFinal)
                    .Select(concurso => concurso.Numero));

        primeiroResultado.Rodadas.Last().ConcursoAlvo.Numero
            .Should()
            .BeLessThan(
                segundoResultado.Rodadas.First().ConcursoAlvo.Numero);

        primeiroResultado.QuantidadeRodadas
            .Should()
            .Be(primeiroPeriodoFinal - primeiroPeriodoInicial + 1);

        segundoResultado.QuantidadeRodadas
            .Should()
            .Be(segundoPeriodoFinal - segundoPeriodoInicial + 1);

        primeiroResultado.QuantidadeJogos
            .Should()
            .Be(primeiroResultado.QuantidadeRodadas);

        segundoResultado.QuantidadeJogos
            .Should()
            .Be(segundoResultado.QuantidadeRodadas);

        primeiroResultado.MediaAcertos.Should().BeInRange(0, 15);
        segundoResultado.MediaAcertos.Should().BeInRange(0, 15);

        primeiroResultado.ObterDistribuicaoAcertos()
            .Values.Sum()
            .Should()
            .Be(primeiroResultado.QuantidadeJogos);

        segundoResultado.ObterDistribuicaoAcertos()
            .Values.Sum()
            .Should()
            .Be(segundoResultado.QuantidadeJogos);
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