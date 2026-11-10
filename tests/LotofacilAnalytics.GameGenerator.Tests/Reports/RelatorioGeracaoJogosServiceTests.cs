
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Reports;

namespace LotofacilAnalytics.GameGenerator.Tests.Reports;

public class RelatorioGeracaoJogosServiceTests
{
    private readonly RelatorioGeracaoJogosService _service = new();

    private static GeracaoJogosResultado CriarResultado(
        int quantidadeSolicitada,
        int quantidadeGerada)
    {
        var jogos = Enumerable.Range(0, quantidadeGerada)
            .Select(indice =>
                Enumerable.Range(1, 14)
                    .Append(15 + indice)
                    .ToArray());

        return new GeracaoJogosResultado(
            quantidadeSolicitada,
            jogos);
    }

    [Fact]
    public void Criar_SemDiversificacao_DeveClassificarTodosComoOriginais()
    {
        var request = new GeracaoJogosRequest(5);
        var resultado = CriarResultado(5, 5);

        var relatorio = _service.Criar(
            "EstrategiaAleatoria",
            request,
            resultado);

        Assert.Equal("EstrategiaAleatoria", relatorio.NomeEstrategia);
        Assert.Equal(5, relatorio.QuantidadeSolicitada);
        Assert.Equal(5, relatorio.QuantidadeGerada);
        Assert.False(relatorio.UtilizouDiversificacao);
        Assert.Equal(5, relatorio.QuantidadeJogosOriginais);
        Assert.Equal(0, relatorio.QuantidadeJogosDiversificados);
    }

    [Fact]
    public void Criar_ComDiversificacao_DeveClassificarUmJogoBase()
    {
        var request = new GeracaoJogosRequest(
            quantidadeJogos: 5,
            diversificarJogos: true);

        var resultado = CriarResultado(5, 5);

        var relatorio = _service.Criar(
            "EstrategiaFrequenciaDezenas",
            request,
            resultado);

        Assert.True(relatorio.UtilizouDiversificacao);
        Assert.Equal(1, relatorio.QuantidadeJogosOriginais);
        Assert.Equal(4, relatorio.QuantidadeJogosDiversificados);
    }

    [Fact]
    public void Criar_ComQuantidadeSolicitadaDiferente_DeveRejeitar()
    {
        var request = new GeracaoJogosRequest(5);
        var resultado = CriarResultado(4, 4);

        Assert.Throws<ArgumentException>(() =>
            _service.Criar(
                "EstrategiaAleatoria",
                request,
                resultado));
    }

    [Fact]
    public void Criar_ComGeracaoIncompleta_DeveRejeitar()
    {
        var request = new GeracaoJogosRequest(5);
        var resultado = CriarResultado(5, 3);

        Assert.Throws<ArgumentException>(() =>
            _service.Criar(
                "EstrategiaAleatoria",
                request,
                resultado));
    }

    [Fact]
    public void Criar_ComRequestNulo_DeveRejeitar()
    {
        var resultado = CriarResultado(5, 5);

        Assert.Throws<ArgumentNullException>(() =>
            _service.Criar(
                "EstrategiaAleatoria",
                null!,
                resultado));
    }

    [Fact]
    public void Criar_ComResultadoNulo_DeveRejeitar()
    {
        var request = new GeracaoJogosRequest(5);

        Assert.Throws<ArgumentNullException>(() =>
            _service.Criar(
                "EstrategiaAleatoria",
                request,
                null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_ComNomeEstrategiaInvalido_DeveRejeitar(
        string? nome)
    {
        var request = new GeracaoJogosRequest(5);
        var resultado = CriarResultado(5, 5);

        Assert.Throws<ArgumentException>(() =>
            _service.Criar(
                nome!,
                request,
                resultado));
    }
}