
using FluentAssertions;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests.Strategies;

public class EstrategiaAleatoriaTests
{
    [Fact]
    public void Deve_gerar_um_jogo_com_15_dezenas_distintas()
    {
        var estrategia = new EstrategiaAleatoria();
        var jogos = estrategia.GerarJogos(CriarHistorico());

        jogos.Should().ContainSingle();

        var jogo = jogos.Single();

        jogo.Should().HaveCount(15);
        jogo.Should().OnlyHaveUniqueItems();
        jogo.Should().OnlyContain(dezena => dezena >= 1 && dezena <= 25);
    }

    [Fact]
    public void Deve_rejeitar_historico_nulo()
    {
        var estrategia = new EstrategiaAleatoria();

        Action acao = () => estrategia.GerarJogos(null!);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_historico_vazio()
    {
        var estrategia = new EstrategiaAleatoria();

        Action acao = () => estrategia.GerarJogos(
            Array.Empty<Concurso>());

        acao.Should()
            .Throw<ArgumentException>()
            .WithMessage("*histórico não pode estar vazio*");
    }

    [Fact]
    public void Deve_reproduzir_o_jogo_com_a_mesma_semente()
    {
        var historico = CriarHistorico();

        var estrategia1 = new EstrategiaAleatoria(123);
        var estrategia2 = new EstrategiaAleatoria(123);

        estrategia1.GerarJogos(historico).Single()
            .Should()
            .Equal(estrategia2.GerarJogos(historico).Single());
    }

    private static IReadOnlyCollection<Concurso> CriarHistorico()
    {
        return new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                Enumerable.Range(1, 15))
        };
    }
}