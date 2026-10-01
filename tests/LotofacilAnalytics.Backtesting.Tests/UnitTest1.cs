using FluentAssertions;
using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests;

public class RodadaBacktestingTests
{
    [Fact]
    public void Deve_criar_uma_rodada_com_os_dados_informados()
    {
        var concursoAlvo = CriarConcurso(6);

        var historicoDisponivel = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var resultadosJogos = new List<ResultadoJogo>
        {
            new(
                new[]
                {
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15
                },
                15),

            new(
                new[]
                {
                    2, 4, 6, 8, 10,
                    12, 14, 16, 18, 20,
                    21, 22, 23, 24, 25
                },
                8)
        };

        var rodada = new RodadaBacktesting(
            concursoAlvo,
            historicoDisponivel,
            resultadosJogos);

        rodada.ConcursoAlvo
            .Should()
            .BeSameAs(concursoAlvo);

        rodada.HistoricoDisponivel
            .Should()
            .BeEquivalentTo(historicoDisponivel);

        rodada.ResultadosJogos
            .Should()
            .BeEquivalentTo(resultadosJogos);
    }

    [Fact]
    public void Deve_rejeitar_concurso_alvo_nulo()
    {
        var historicoDisponivel = new List<Concurso>();

        var resultadosJogos = new List<ResultadoJogo>();

        Action acao = () => new RodadaBacktesting(
            null!,
            historicoDisponivel,
            resultadosJogos);

        acao.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_historico_disponivel_nulo()
    {
        var concursoAlvo = CriarConcurso(6);

        var resultadosJogos = new List<ResultadoJogo>();

        Action acao = () => new RodadaBacktesting(
            concursoAlvo,
            null!,
            resultadosJogos);

        acao.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_resultados_jogos_nulo()
    {
        var concursoAlvo = CriarConcurso(6);

        var historicoDisponivel = new List<Concurso>();

        Action acao = () => new RodadaBacktesting(
            concursoAlvo,
            historicoDisponivel,
            null!);

        acao.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_preservar_o_historico_informado_na_criacao_da_rodada()
    {
        var concursoAlvo = CriarConcurso(6);

        var historicoDisponivel = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var resultadosJogos = new List<ResultadoJogo>
        {
            new(
                Enumerable.Range(1, 15),
                15)
        };

        var rodada = new RodadaBacktesting(
            concursoAlvo,
            historicoDisponivel,
            resultadosJogos);

        historicoDisponivel.Clear();

        rodada.HistoricoDisponivel
            .Should()
            .HaveCount(5);
    }

    [Fact]
    public void Deve_preservar_os_resultados_dos_jogos_na_criacao_da_rodada()
    {
        var concursoAlvo = CriarConcurso(6);

        var historicoDisponivel = new List<Concurso>
        {
            CriarConcurso(1),
            CriarConcurso(2),
            CriarConcurso(3),
            CriarConcurso(4),
            CriarConcurso(5)
        };

        var resultadosJogos = new List<ResultadoJogo>
        {
            new(
                Enumerable.Range(1, 15),
                15),

            new(
                Enumerable.Range(11, 15),
                5)
        };

        var rodada = new RodadaBacktesting(
            concursoAlvo,
            historicoDisponivel,
            resultadosJogos);

        resultadosJogos.Clear();

        rodada.ResultadosJogos
            .Should()
            .HaveCount(2);
    }

    [Fact]
    public void Deve_armazenar_os_resultados_dos_jogos_avaliados()
    {
        var concursoAlvo = CriarConcurso(6);

        var historicoDisponivel = new List<Concurso>();

        var resultados = new List<ResultadoJogo>
        {
            new(Enumerable.Range(1, 15), 8),
            new(Enumerable.Range(2, 15), 10)
        };

        var rodada = new RodadaBacktesting(
            concursoAlvo,
            historicoDisponivel,
            resultados);

        rodada.ResultadosJogos
            .Should()
            .BeEquivalentTo(resultados);
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