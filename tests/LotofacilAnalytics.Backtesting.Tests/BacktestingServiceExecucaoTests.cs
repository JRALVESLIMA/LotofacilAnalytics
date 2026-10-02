using FluentAssertions;
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests;

public class BacktestingServiceExecucaoTests
{
    [Fact]
    public void Deve_executar_uma_rodada_para_cada_concurso_com_historico_anterior()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        var resultados = service.Executar(
            historico,
            estrategia);

        resultados
            .Should()
            .HaveCount(4);

        resultados
            .First()
            .ConcursoAlvo.Numero
            .Should()
            .Be(2);

        resultados
            .Last()
            .ConcursoAlvo.Numero
            .Should()
            .Be(5);
    }

    [Fact]
    public void Deve_fornecer_para_cada_rodada_apenas_o_historico_anterior_ao_concurso_alvo()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        service.Executar(
            historico,
            estrategia);

        estrategia.HistoricosRecebidos
            .Select(h => h.Select(c => c.Numero))
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    new[] { 1 },
                    new[] { 1, 2 },
                    new[] { 1, 2, 3 },
                    new[] { 1, 2, 3, 4 }
                });
    }

    [Fact]
    public void Deve_executar_as_rodadas_em_ordem_crescente_de_concurso()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(5),
            CriarConcurso(4),
            CriarConcurso(3),
            CriarConcurso(2),
            CriarConcurso(1)
        };

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        var resultados = service.Executar(
            historico,
            estrategia);

        resultados
            .Select(resultado => resultado.ConcursoAlvo.Numero)
            .Should()
            .Equal(2, 3, 4, 5);
    }

    [Fact]
    public void Deve_retornar_nenhuma_rodada_quando_houver_apenas_um_concurso()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1)
        };

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        var resultados = service.Executar(
            historico,
            estrategia);

        resultados
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void Deve_rejeitar_historico_nulo()
    {
        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        Action acao = () => service.Executar(
            null!,
            estrategia);

        acao
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_estrategia_nula()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2)
        };

        var service = new BacktestingService();

        Action acao = () => service.Executar(
            historico,
            null!);

        acao
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_avaliar_os_jogos_gerados_em_cada_rodada()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3)
        };

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        var resultados = service.Executar(
            historico,
            estrategia);

        resultados.Should().HaveCount(2);

        resultados[0].ResultadosJogos.Should().HaveCount(1);
        resultados[0].ResultadosJogos.Single().QuantidadeAcertos.Should().Be(14);

        resultados[1].ResultadosJogos.Should().HaveCount(1);
        resultados[1].ResultadosJogos.Single().QuantidadeAcertos.Should().Be(13);
    }

    [Fact]
    public void Deve_preservar_os_jogos_gerados_em_cada_rodada()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3)
        };

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        var resultados = service.Executar(
            historico,
            estrategia);

        resultados.Should().HaveCount(2);

        resultados[0].ResultadosJogos.Single().Jogo
            .Should()
            .BeEquivalentTo(Enumerable.Range(2, 15));

        resultados[1].ResultadosJogos.Single().Jogo
            .Should()
            .BeEquivalentTo(Enumerable.Range(3, 15));
    }

    [Fact]
    public void Deve_retornar_nenhuma_rodada_quando_historico_estiver_vazio()
    {
        var historico = new List<Concurso>();

        var estrategia = new EstrategiaFake();

        var service = new BacktestingService();

        var resultados = service.Executar(
            historico,
            estrategia);

        resultados.Should().BeEmpty();
    }

    private static Concurso CriarConcurso(int numero)
    {
        return new Concurso(
            numero,
            new DateTime(2026, 1, numero),
            Enumerable.Range(1, 15));
    }

    private sealed class EstrategiaFake : IEstrategia
    {
        private readonly List<IReadOnlyCollection<Concurso>> _historicosRecebidos = new();

        public IReadOnlyCollection<IReadOnlyCollection<Concurso>> HistoricosRecebidos =>
            _historicosRecebidos;

        public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
            IReadOnlyCollection<Concurso> historico)
        {
            _historicosRecebidos.Add(historico);

            var inicio = historico.Count + 1;

            return new[]
            {
                (IReadOnlyCollection<int>)Enumerable
                    .Range(inicio, 15)
                    .ToArray()
            };
        }
    }
}