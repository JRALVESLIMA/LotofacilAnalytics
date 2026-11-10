
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Services;
using LotofacilAnalytics.GameGenerator.Tests.Fixtures;

namespace LotofacilAnalytics.GameGenerator.Tests.Integration;

public class GeradorJogosDiversificacaoTests
{
    private readonly GeradorJogosService _gerador = new();

    [Fact]
    public void Gerar_ComDiversificacao_DeveGerarQuantidadeSolicitada()
    {
        var resultado = _gerador.Gerar(
            ConcursoFixture.CriarHistorico(),
            new EstrategiaFrequenciaDezenas(),
            new GeracaoJogosRequest(
                quantidadeJogos: 10,
                diversificarJogos: true));

        Assert.Equal(10, resultado.QuantidadeGerada);
    }

    [Fact]
    public void Gerar_ComDiversificacao_DevePreservarJogoBase()
    {
        var historico = ConcursoFixture.CriarHistorico();
        var estrategia = new EstrategiaFrequenciaDezenas();

        var jogoBase = estrategia.GerarJogos(historico).Single();

        var resultado = _gerador.Gerar(
            historico,
            estrategia,
            new GeracaoJogosRequest(
                quantidadeJogos: 5,
                diversificarJogos: true));

        Assert.Equal(jogoBase, resultado.Jogos.First());
    }

    [Fact]
    public void Gerar_ComDiversificacao_NaoDeveGerarJogosDuplicados()
    {
        var resultado = _gerador.Gerar(
            ConcursoFixture.CriarHistorico(),
            new EstrategiaFrequenciaDezenas(),
            new GeracaoJogosRequest(
                quantidadeJogos: 100,
                diversificarJogos: true));

        var chaves = resultado.Jogos
            .Select(jogo => string.Join(",", jogo.Order()))
            .ToArray();

        Assert.Equal(
            chaves.Length,
            chaves.Distinct().Count());
    }

    [Fact]
    public void Gerar_ComDiversificacao_DeveGerarJogosValidos()
    {
        var resultado = _gerador.Gerar(
            ConcursoFixture.CriarHistorico(),
            new EstrategiaFrequenciaDezenas(),
            new GeracaoJogosRequest(
                quantidadeJogos: 20,
                diversificarJogos: true));

        Assert.All(resultado.Jogos, jogo =>
        {
            Assert.Equal(15, jogo.Count);
            Assert.Equal(15, jogo.Distinct().Count());

            Assert.All(jogo, dezena =>
                Assert.InRange(dezena, 1, 25));
        });
    }

    [Fact]
    public void Gerar_ComMesmaSemente_DeveProduzirMesmosJogos()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var request = new GeracaoJogosRequest(
            quantidadeJogos: 20,
            diversificarJogos: true,
            sementeDiversificacao: 98765);

        var primeiro = _gerador.Gerar(
            historico,
            new EstrategiaFrequenciaDezenas(),
            request);

        var segundo = _gerador.Gerar(
            historico,
            new EstrategiaFrequenciaDezenas(),
            request);

        Assert.Equal(
            primeiro.Jogos.Select(j => string.Join(",", j)),
            segundo.Jogos.Select(j => string.Join(",", j)));
    }

    [Fact]
    public void Gerar_SemDiversificacao_DevePreservarComportamentoAnterior()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var resultado = _gerador.Gerar(
            historico,
            new EstrategiaFrequenciaDezenas(),
            new GeracaoJogosRequest(1));

        Assert.Single(resultado.Jogos);
        Assert.Equal(
            Enumerable.Range(1, 15),
            resultado.Jogos.Single());
    }

    [Fact]
    public void Gerar_SemDiversificacao_DeveRejeitarMultiplosJogosIguais()
    {
        var historico = ConcursoFixture.CriarHistorico();

        Assert.Throws<InvalidOperationException>(() =>
            _gerador.Gerar(
                historico,
                new EstrategiaFrequenciaDezenas(),
                new GeracaoJogosRequest(3)));
    }
}