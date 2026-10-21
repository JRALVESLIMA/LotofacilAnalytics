
using FluentAssertions;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests.Strategies;

public class EstrategiaFrequenciaDezenasTests
{
    [Fact]
    public void Deve_Gerar_Um_Jogo_Com_15_Dezenas_Distintas()
    {
        var historico = CriarHistorico();
        var estrategia = new EstrategiaFrequenciaDezenas();

        var jogos = estrategia.GerarJogos(historico);

        jogos.Should().ContainSingle();

        var jogo = jogos.Single();

        jogo.Should().HaveCount(15);
        jogo.Should().OnlyHaveUniqueItems();
        jogo.Should().OnlyContain(dezena => dezena >= 1 && dezena <= 25);
    }

    [Fact]
    public void Deve_Priorizar_Dezenas_Com_Maior_Frequencia()
    {
        var historico = CriarHistorico();
        var estrategia = new EstrategiaFrequenciaDezenas();

        var jogos = estrategia.GerarJogos(historico);

        var jogo = jogos.Single();

        jogo.Should().BeEquivalentTo(Enumerable.Range(1, 15));
    }

    [Fact]
    public void Deve_Resolver_Empate_Na_Decima_Quinta_Posicao_Pela_Dezena_Menor()
    {
        var historico = new[]
        {
            CriarConcurso(1, Enumerable.Range(1, 13).Concat([14, 15])),
            CriarConcurso(2, Enumerable.Range(1, 13).Concat([14, 16]))
        };

        var estrategia = new EstrategiaFrequenciaDezenas();

        var jogos = estrategia.GerarJogos(historico);

        var jogo = jogos.Single();

        jogo.Should().HaveCount(15);
        jogo.Should().Contain(14);
        jogo.Should().Contain(15);
        jogo.Should().NotContain(16);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Historico_Estiver_Vazio()
    {
        var estrategia = new EstrategiaFrequenciaDezenas();

        var acao = () => estrategia.GerarJogos(
            Array.Empty<Concurso>());

        acao.Should().Throw<ArgumentException>();
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