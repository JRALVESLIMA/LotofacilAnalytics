using FluentAssertions;
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests;

public class BacktestingServiceTests
{
    [Fact]
    public void Deve_fornecer_apenas_os_concursos_anteriores_ao_concurso_alvo()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5),
            CriarConcurso(6),
            CriarConcurso(7),
            CriarConcurso(8),
            CriarConcurso(9),
            CriarConcurso(10)
        };

        var concursoAlvo = historico.Single(c => c.Numero == 6);

        var service = new BacktestingService();

        var historicoDisponivel =
            service.ObterHistoricoDisponivel(
                historico,
                concursoAlvo);

        historicoDisponivel
            .Select(c => c.Numero)
            .Should()
            .Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Deve_retornar_historico_vazio_quando_o_alvo_for_o_primeiro_concurso()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var concursoAlvo = historico.Single(c => c.Numero == 1);

        var service = new BacktestingService();

        var historicoDisponivel =
            service.ObterHistoricoDisponivel(
                historico,
                concursoAlvo);

        historicoDisponivel.Should().BeEmpty();
    }

    [Fact]
    public void Deve_ignorar_concursos_posteriores_mesmo_com_historico_fora_de_ordem()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(10),
            CriarConcurso(3),
            CriarConcurso(7),
            CriarConcurso(1),
            CriarConcurso(8),
            CriarConcurso(5),
            CriarConcurso(2),
            CriarConcurso(9),
            CriarConcurso(6),
            CriarConcurso(4)
        };

        var concursoAlvo = historico.Single(c => c.Numero == 6);

        var service = new BacktestingService();

        var historicoDisponivel =
            service.ObterHistoricoDisponivel(
                historico,
                concursoAlvo);

        historicoDisponivel
            .Select(c => c.Numero)
            .Should()
            .BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public void Deve_preparar_uma_rodada_com_historico_anterior_ao_alvo()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5),
            CriarConcurso(6)
        };

        var concursoAlvo = CriarConcurso(6);

        var jogosGerados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray()
        };

        var service = new BacktestingService();

        var rodada = service.PrepararRodada(
            historico,
            concursoAlvo,
            new EstrategiaFake(jogosGerados));

        rodada.ConcursoAlvo
            .Should()
            .BeSameAs(concursoAlvo);

        rodada.HistoricoDisponivel
            .Select(concurso => concurso.Numero)
            .Should()
            .BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });

        rodada.ResultadosJogos
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void Deve_rejeitar_historico_nulo_ao_preparar_uma_rodada()
    {
        var concursoAlvo = CriarConcurso(6);

        var jogosGerados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray()
        };

        var service = new BacktestingService();

        Action acao = () => service.PrepararRodada(
            null!,
            concursoAlvo,
            new EstrategiaFake(jogosGerados));

        acao.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_concurso_alvo_nulo_ao_preparar_uma_rodada()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2)
        };

        var jogosGerados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray()
        };

        var service = new BacktestingService();

        Action acao = () => service.PrepararRodada(
            historico,
            null!,
            new EstrategiaFake(jogosGerados));

        acao.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_ignorar_concursos_posteriores_ao_alvo_ao_preparar_uma_rodada()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5),
            CriarConcurso(6),
            CriarConcurso(7),
            CriarConcurso(8)
        };

        var concursoAlvo = CriarConcurso(6);

        var jogosGerados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray()
        };

        var service = new BacktestingService();

        var rodada = service.PrepararRodada(
            historico,
            concursoAlvo,
            new EstrategiaFake(jogosGerados));

        rodada.HistoricoDisponivel
            .Select(concurso => concurso.Numero)
            .Should()
            .BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public void Deve_usar_a_estrategia_para_gerar_os_jogos_da_rodada()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var concursoAlvo = CriarConcurso(6);

        var jogosEsperados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray(),
            Enumerable.Range(2, 15).ToArray()
        };

        var estrategia = new EstrategiaFake(jogosEsperados);

        var service = new BacktestingService();

        var rodada = service.PrepararRodada(
            historico,
            concursoAlvo,
            estrategia);

        rodada.ResultadosJogos
            .Select(resultado => resultado.Jogo)
            .Should()
            .BeEquivalentTo(jogosEsperados);
    }

    [Fact]
    public void Deve_fornecer_a_estrategia_apenas_o_historico_disponivel()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5),
            CriarConcurso(6),
            CriarConcurso(7),
            CriarConcurso(8)
        };

        var concursoAlvo = CriarConcurso(6);

        var jogosGerados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray()
        };

        var estrategia = new EstrategiaFake(jogosGerados);

        var service = new BacktestingService();

        service.PrepararRodada(
            historico,
            concursoAlvo,
            estrategia);

        estrategia.HistoricoRecebido!
            .Select(concurso => concurso.Numero)
            .Should()
            .BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public void Deve_avaliar_os_jogos_gerados_contra_o_resultado_real()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var concursoAlvo = CriarConcurso(6);

        var jogosGerados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray()
        };

        var estrategia = new EstrategiaFake(jogosGerados);

        var service = new BacktestingService();

        var rodada = service.PrepararRodada(
            historico,
            concursoAlvo,
            estrategia);

        rodada.ResultadosJogos
            .Should()
            .HaveCount(1);

        rodada.ResultadosJogos
            .Single()
            .QuantidadeAcertos
            .Should()
            .Be(15);
    }

    private sealed class EstrategiaFake : IEstrategia
    {
        private readonly IReadOnlyCollection<IReadOnlyCollection<int>> _jogos;

        private IReadOnlyCollection<Concurso>? _historicoRecebido;

        public EstrategiaFake(
            IReadOnlyCollection<IReadOnlyCollection<int>> jogos)
        {
            _jogos = jogos;
        }

        public IReadOnlyCollection<Concurso>? HistoricoRecebido =>
            _historicoRecebido;

        public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
            IReadOnlyCollection<Concurso> historico)
        {
            _historicoRecebido = historico;

            return _jogos;
        }
    }

    [Fact]
    public void Deve_avaliar_varios_jogos_com_quantidades_de_acertos_diferentes()
    {
        var historico = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var concursoAlvo = CriarConcurso(6);

        var jogosGerados = new List<IReadOnlyCollection<int>>
        {
            Enumerable.Range(1, 15).ToArray(),
            Enumerable.Range(11, 15).ToArray()
        };

        var estrategia = new EstrategiaFake(jogosGerados);

        var service = new BacktestingService();

        var rodada = service.PrepararRodada(
            historico,
            concursoAlvo,
            estrategia);

        rodada.ResultadosJogos
            .Select(resultado => resultado.QuantidadeAcertos)
            .Should()
            .BeEquivalentTo(new[] { 15, 5 });
    }

    private static Concurso CriarConcurso(int numero)
    {
        var dezenas = Enumerable.Range(1, 15);

        return new Concurso(
            numero,
            new DateTime(2026, 1, numero),
            dezenas);
    }
}