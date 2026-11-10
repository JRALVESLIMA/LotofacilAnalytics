
using LotofacilAnalytics.GameGenerator.Reports;

namespace LotofacilAnalytics.GameGenerator.Tests.Reports;

public class RelatorioGeracaoJogosTests
{
    [Fact]
    public void Construtor_SemDiversificacao_DeveArmazenarDados()
    {
        var relatorio = new RelatorioGeracaoJogos(
            nomeEstrategia: "EstrategiaAleatoria",
            quantidadeSolicitada: 5,
            quantidadeGerada: 5,
            utilizouDiversificacao: false,
            quantidadeJogosOriginais: 5);

        Assert.Equal("EstrategiaAleatoria", relatorio.NomeEstrategia);
        Assert.Equal(5, relatorio.QuantidadeSolicitada);
        Assert.Equal(5, relatorio.QuantidadeGerada);
        Assert.False(relatorio.UtilizouDiversificacao);
        Assert.Equal(5, relatorio.QuantidadeJogosOriginais);
        Assert.Equal(0, relatorio.QuantidadeJogosDiversificados);
    }

    [Fact]
    public void Construtor_ComDiversificacao_DeveCalcularJogosDiversificados()
    {
        var relatorio = new RelatorioGeracaoJogos(
            nomeEstrategia: "EstrategiaFrequenciaDezenas",
            quantidadeSolicitada: 10,
            quantidadeGerada: 10,
            utilizouDiversificacao: true,
            quantidadeJogosOriginais: 1);

        Assert.True(relatorio.UtilizouDiversificacao);
        Assert.Equal(1, relatorio.QuantidadeJogosOriginais);
        Assert.Equal(9, relatorio.QuantidadeJogosDiversificados);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_DeveRejeitarNomeEstrategiaInvalido(
        string? nome)
    {
        Assert.Throws<ArgumentException>(() =>
            new RelatorioGeracaoJogos(
                nome!,
                5,
                5,
                false,
                5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Construtor_DeveRejeitarQuantidadeSolicitadaInvalida(
        int quantidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new RelatorioGeracaoJogos(
                "EstrategiaAleatoria",
                quantidade,
                0,
                false,
                0));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void Construtor_DeveRejeitarQuantidadeGeradaInvalida(
        int quantidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new RelatorioGeracaoJogos(
                "EstrategiaAleatoria",
                5,
                quantidade,
                false,
                0));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void Construtor_DeveRejeitarQuantidadeJogosOriginaisInvalida(
        int quantidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new RelatorioGeracaoJogos(
                "EstrategiaAleatoria",
                5,
                5,
                false,
                quantidade));
    }

    [Fact]
    public void Construtor_SemDiversificacao_DeveExigirTodosJogosOriginais()
    {
        Assert.Throws<ArgumentException>(() =>
            new RelatorioGeracaoJogos(
                "EstrategiaAleatoria",
                5,
                5,
                false,
                1));
    }

    [Fact]
    public void Construtor_ComDiversificacao_DeveExigirUmJogoBase()
    {
        Assert.Throws<ArgumentException>(() =>
            new RelatorioGeracaoJogos(
                "EstrategiaFrequenciaDezenas",
                5,
                5,
                true,
                2));
    }
}