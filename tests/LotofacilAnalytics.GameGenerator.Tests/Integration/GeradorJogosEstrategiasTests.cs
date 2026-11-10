
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Services;
using LotofacilAnalytics.GameGenerator.Tests.Fixtures;

namespace LotofacilAnalytics.GameGenerator.Tests.Integration;

public class GeradorJogosEstrategiasTests
{
    private readonly GeradorJogosService _gerador = new();

    [Fact]
    public void Gerar_ComFrequenciaGeral_DeveConsiderarTodoHistorico()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var estrategia = new EstrategiaFrequenciaDezenas();

        var request = new GeracaoJogosRequest(1);

        var resultado = _gerador.Gerar(
            historico,
            estrategia,
            request);

        Assert.Equal(1, resultado.QuantidadeGerada);

        Assert.Equal(
            Enumerable.Range(1, 15),
            resultado.Jogos.Single());
    }

    [Fact]
    public void Gerar_ComFrequenciaRecente_DeveConsiderarUltimosDoisConcursos()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var estrategia = new EstrategiaFrequenciaRecente(
            quantidadeConcursos: 2);

        var request = new GeracaoJogosRequest(1);

        var resultado = _gerador.Gerar(
            historico,
            estrategia,
            request);

        Assert.Equal(1, resultado.QuantidadeGerada);

        Assert.Equal(
            Enumerable.Range(11, 15),
            resultado.Jogos.Single());
    }

    [Fact]
    public void Gerar_ComFrequenciaGeral_DeveRejeitarTresJogosDistintos()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var estrategia = new EstrategiaFrequenciaDezenas();

        var request = new GeracaoJogosRequest(3);

        Assert.Throws<InvalidOperationException>(() =>
            _gerador.Gerar(
                historico,
                estrategia,
                request));
    }

    [Fact]
    public void Gerar_ComFrequenciaRecente_DevePermitirJogosRepetidos()
    {
        var historico = ConcursoFixture.CriarHistorico();

        var estrategia = new EstrategiaFrequenciaRecente(
            quantidadeConcursos: 2);

        var request = new GeracaoJogosRequest(
            quantidadeJogos: 3,
            permitirJogosRepetidos: true);

        var resultado = _gerador.Gerar(
            historico,
            estrategia,
            request);

        Assert.Equal(3, resultado.QuantidadeGerada);

        Assert.All(resultado.Jogos, jogo =>
            Assert.Equal(
                Enumerable.Range(11, 15),
                jogo));
    }
}