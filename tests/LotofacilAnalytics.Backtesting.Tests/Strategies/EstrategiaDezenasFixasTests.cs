
using FluentAssertions;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests.Strategies;

public class EstrategiaDezenasFixasTests
{
    [Fact]
    public void Deve_gerar_um_jogo_com_as_dezenas_de_1_a_15()
    {
        var estrategia = new EstrategiaDezenasFixas();
        var historico = CriarHistorico();

        var jogos = estrategia.GerarJogos(historico);

        jogos.Should().ContainSingle();
        jogos.Single().Should().Equal(
            Enumerable.Range(1, 15));
    }

    [Fact]
    public void Deve_rejeitar_historico_nulo()
    {
        var estrategia = new EstrategiaDezenasFixas();

        Action acao = () => estrategia.GerarJogos(null!);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_historico_vazio()
    {
        var estrategia = new EstrategiaDezenasFixas();

        Action acao = () => estrategia.GerarJogos(
            Array.Empty<Concurso>());

        acao.Should()
            .Throw<ArgumentException>()
            .WithMessage("*histórico não pode estar vazio*");
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