
using FluentAssertions;
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests.Strategies;

public class EstrategiaFrequenciaRecenteTests
{
    [Fact]
    public void Deve_Gerar_Um_Jogo_Com_15_Dezenas_Distintas()
    {
        var historico = CriarHistorico();
        var estrategia = new EstrategiaFrequenciaRecente();

        var jogos = estrategia.GerarJogos(historico);

        jogos.Should().ContainSingle();

        var jogo = jogos.Single();

        jogo.Should().HaveCount(15);
        jogo.Should().OnlyHaveUniqueItems();
        jogo.Should().OnlyContain(dezena => dezena >= 1 && dezena <= 25);
    }

    
    [Fact]
    public void Deve_Priorizar_Dezenas_Com_Maior_Frequencia_Recente()
    {
        var historico = new[]
        {
            CriarConcurso(1, Enumerable.Range(1, 15)),
            CriarConcurso(2, Enumerable.Range(11, 15)),
            CriarConcurso(3, Enumerable.Range(1, 15))
        };

        var estrategia = new EstrategiaFrequenciaRecente(
            quantidadeConcursos: 1);

        var jogos = estrategia.GerarJogos(historico);

        var jogo = jogos.Single();

        jogo.Should().BeEquivalentTo(Enumerable.Range(1, 15));
    }

    [Fact]
    public void Deve_Considerar_Apenas_A_Quantidade_De_Concursos_Configurada()
    {
        var historico = new[]
        {
            CriarConcurso(1, Enumerable.Range(1, 15)),
            CriarConcurso(2, Enumerable.Range(1, 15)),
            CriarConcurso(3, Enumerable.Range(11, 15))
        };

        var estrategia = new EstrategiaFrequenciaRecente(
            quantidadeConcursos: 1);

        var jogos = estrategia.GerarJogos(historico);

        var jogo = jogos.Single();

        jogo.Should().BeEquivalentTo(Enumerable.Range(11, 15));
    }

    [Fact]
    public void Deve_Resolver_Empate_Pela_Dezena_Menor()
    {
        var historico = new[]
        {
            CriarConcurso(
                1,
                Enumerable.Range(1, 13).Concat([14, 15])),

            CriarConcurso(
                2,
                Enumerable.Range(1, 13).Concat([14, 16]))
        };

        var estrategia = new EstrategiaFrequenciaRecente(
            quantidadeConcursos: 2);

        var jogos = estrategia.GerarJogos(historico);

        var jogo = jogos.Single();

        jogo.Should().HaveCount(15);
        jogo.Should().Contain(14);
        jogo.Should().Contain(15);
        jogo.Should().NotContain(16);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Historico_For_Nulo()
    {
        var estrategia = new EstrategiaFrequenciaRecente();

        var acao = () => estrategia.GerarJogos(null!);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Historico_Estiver_Vazio()
    {
        var estrategia = new EstrategiaFrequenciaRecente();

        var acao = () => estrategia.GerarJogos(
            Array.Empty<Concurso>());

        acao.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Quantidade_De_Concursos_For_Invalida()
    {
        var acao = () => new EstrategiaFrequenciaRecente(0);

        acao.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Deve_Executar_Estrategia_Recente_E_Consolidar_Resultados()
    {
        var historico = Enumerable
            .Range(1, 5)
            .Select(numero => CriarConcurso(
                numero,
                Enumerable.Range(1, 15)))
            .ToList();

        var estrategia = new EstrategiaFrequenciaRecente();
        var service = new BacktestingService();

        var resultado = service.ExecutarConsolidado(
            historico,
            estrategia);

        resultado.QuantidadeRodadas.Should().Be(4);
        resultado.QuantidadeJogos.Should().Be(4);

        resultado.Rodadas.Should().HaveCount(4);

        resultado.Rodadas.Should().OnlyContain(
            rodada => rodada.ResultadosJogos.Count == 1);

        resultado.ObterDistribuicaoAcertos()
            .Values
            .Sum()
            .Should()
            .Be(4);
    }

    private static List<Concurso> CriarHistorico()
    {
        return
        [
            CriarConcurso(1, Enumerable.Range(1, 15)),
            CriarConcurso(2, Enumerable.Range(1, 15))
        ];
    }

    private static Concurso CriarConcurso(
        int numero,
        IEnumerable<int> dezenas)
    {
        return new Concurso(
            numero,
            new DateTime(2026, 1, numero),
            dezenas.ToArray());
    }
}