
using LotofacilAnalytics.GameGenerator.Models;

namespace LotofacilAnalytics.GameGenerator.Tests.Models;

public class GeracaoJogosRequestTests
{
    [Fact]
    public void Construtor_DeveAceitarQuantidadePositiva()
    {
        var request = new GeracaoJogosRequest(5);

        Assert.Equal(5, request.QuantidadeJogos);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Construtor_DeveRejeitarQuantidadeInvalida(
        int quantidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GeracaoJogosRequest(quantidade));
    }

    [Fact]
    public void Construtor_DeveDesabilitarJogosRepetidosPorPadrao()
    {
        var request = new GeracaoJogosRequest(5);

        Assert.False(request.PermitirJogosRepetidos);
    }

    [Fact]
    public void Construtor_DevePermitirHabilitarJogosRepetidos()
    {
        var request = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            permitirJogosRepetidos: true);

        Assert.True(request.PermitirJogosRepetidos);
    }

    [Fact]
    public void Construtor_DeveAceitarQuantidadeMaxima()
    {
        var request = new GeracaoJogosRequest(
            GeracaoJogosRequest.QuantidadeMaximaJogos);

        Assert.Equal(
            GeracaoJogosRequest.QuantidadeMaximaJogos,
            request.QuantidadeJogos);
    }

    [Theory]
    [InlineData(101)]
    [InlineData(1000)]
    public void Construtor_DeveRejeitarQuantidadeAcimaDoLimite(
        int quantidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GeracaoJogosRequest(quantidade));
    }

    [Fact]
    public void Construtor_DeveDesabilitarDiversificacaoPorPadrao()
    {
        var request = new GeracaoJogosRequest(5);

        Assert.False(request.DiversificarJogos);
    }

    [Fact]
    public void Construtor_DevePermitirHabilitarDiversificacao()
    {
        var request = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            diversificarJogos: true);

        Assert.True(request.DiversificarJogos);
    }

    [Fact]
    public void Construtor_DeveArmazenarSementeDiversificacao()
    {
        var request = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            diversificarJogos: true,
            sementeDiversificacao: 98765);

        Assert.Equal(98765, request.SementeDiversificacao);
    }

    [Fact]
    public void Construtor_DeveRejeitarDiversificacaoComRepeticao()
    {
        Assert.Throws<ArgumentException>(() =>
            new GeracaoJogosRequest(
                quantidadeJogos: 5,
                permitirJogosRepetidos: true,
                diversificarJogos: true));
    }
}