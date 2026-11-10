
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Services;
using LotofacilAnalytics.GameGenerator.Tests.Fixtures;

namespace LotofacilAnalytics.GameGenerator.Tests.Services;

public class GeradorJogosServiceTests
{
    private readonly GeradorJogosService _service = new();

    private static IReadOnlyCollection<Concurso> CriarHistorico()
    {
        return ConcursoFixture.CriarHistorico();
    }

    [Fact]
    public void Gerar_DeveGerarCincoJogosAleatoriosDistintos()
    {
        var estrategia = new EstrategiaAleatoria(semente: 12345);
        var request = new GeracaoJogosRequest(5);

        var resultado = _service.Gerar(
            CriarHistorico(),
            estrategia,
            request);

        Assert.Equal(5, resultado.QuantidadeSolicitada);
        Assert.Equal(5, resultado.QuantidadeGerada);

        Assert.All(resultado.Jogos, jogo =>
        {
            Assert.Equal(15, jogo.Count);
            Assert.Equal(15, jogo.Distinct().Count());
            Assert.All(jogo, dezena =>
                Assert.InRange(dezena, 1, 25));
        });

        Assert.Equal(
            5,
            resultado.Jogos
                .Select(jogo => string.Join(",", jogo))
                .Distinct()
                .Count());
    }

    [Fact]
    public void Gerar_DeveAceitarUmJogoDeDezenasFixas()
    {
        var resultado = _service.Gerar(
            CriarHistorico(),
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(1));

        Assert.Equal(1, resultado.QuantidadeGerada);
        Assert.Equal(
            Enumerable.Range(1, 15),
            resultado.Jogos.Single());
    }

    [Fact]
    public void Gerar_DeveRejeitarQuantidadeImpossivelSemRepeticao()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _service.Gerar(
                CriarHistorico(),
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(3)));
    }

    [Fact]
    public void Gerar_DevePermitirJogosRepetidosQuandoConfigurado()
    {
        var resultado = _service.Gerar(
            CriarHistorico(),
            new EstrategiaDezenasFixas(),
            new GeracaoJogosRequest(
                quantidadeJogos: 3,
                permitirJogosRepetidos: true));

        Assert.Equal(3, resultado.QuantidadeGerada);
        Assert.All(resultado.Jogos, jogo =>
            Assert.Equal(
                Enumerable.Range(1, 15),
                jogo));
    }

    [Fact]
    public void Gerar_DeveRejeitarHistoricoVazio()
    {
        Assert.Throws<ArgumentException>(() =>
            _service.Gerar(
                Array.Empty<Concurso>(),
                new EstrategiaDezenasFixas(),
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Gerar_DeveRejeitarEstrategiaNula()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _service.Gerar(
                CriarHistorico(),
                null!,
                new GeracaoJogosRequest(1)));
    }

    [Fact]
    public void Gerar_DeveRejeitarJogoInvalido()
    {
        var estrategia = new EstrategiaInvalida();

        Assert.Throws<ArgumentException>(() =>
            _service.Gerar(
                CriarHistorico(),
                estrategia,
                new GeracaoJogosRequest(1)));
    }

    private sealed class EstrategiaInvalida : IEstrategia
    {
        public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
            IReadOnlyCollection<Concurso> historico)
        {
            IReadOnlyCollection<int> jogo =
                Enumerable.Range(1, 14).ToArray();

            return new[] { jogo };
        }
    }
}