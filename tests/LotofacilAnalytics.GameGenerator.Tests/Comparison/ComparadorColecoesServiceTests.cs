
using LotofacilAnalytics.GameGenerator.Comparison;

namespace LotofacilAnalytics.GameGenerator.Tests.Comparison;

public class ComparadorColecoesServiceTests
{
    private readonly ComparadorColecoesService _service = new();

    private static IReadOnlyCollection<IReadOnlyCollection<int>>
        CriarColecao(params int[][] jogos)
    {
        return jogos
            .Select(jogo => (IReadOnlyCollection<int>)jogo)
            .ToArray();
    }

    private static int[] CriarJogo(int inicio)
    {
        return Enumerable.Range(inicio, 15).ToArray();
    }

    [Fact]
    public void Comparar_ColecoesIdenticas_DeveRetornarTodosComuns()
    {
        var jogosA = CriarColecao(
            CriarJogo(1),
            CriarJogo(2));

        var jogosB = CriarColecao(
            CriarJogo(1),
            CriarJogo(2));

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogosA,
            "EstrategiaB",
            jogosB);

        Assert.Equal(2, resultado.QuantidadeJogosA);
        Assert.Equal(2, resultado.QuantidadeJogosB);
        Assert.Equal(2, resultado.QuantidadeJogosComuns);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ColecoesParcialmenteIguais_DeveCalcularExclusivos()
    {
        var jogosA = CriarColecao(
            CriarJogo(1),
            CriarJogo(2),
            CriarJogo(3));

        var jogosB = CriarColecao(
            CriarJogo(2),
            CriarJogo(4));

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogosA,
            "EstrategiaB",
            jogosB);

        Assert.Equal(3, resultado.QuantidadeJogosA);
        Assert.Equal(2, resultado.QuantidadeJogosB);
        Assert.Equal(1, resultado.QuantidadeJogosComuns);
        Assert.Equal(2, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(1, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_SemCombinacoesComuns_DeveRetornarZero()
    {
        var jogosA = CriarColecao(CriarJogo(1));
        var jogosB = CriarColecao(CriarJogo(11));

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogosA,
            "EstrategiaB",
            jogosB);

        Assert.Equal(0, resultado.QuantidadeJogosComuns);
        Assert.Equal(1, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(1, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_DeveIgnorarOrdemDasDezenas()
    {
        var jogosA = CriarColecao(CriarJogo(1));

        var jogosB = CriarColecao(
            CriarJogo(1).Reverse().ToArray());

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogosA,
            "EstrategiaB",
            jogosB);

        Assert.Equal(1, resultado.QuantidadeJogosComuns);
    }

    [Fact]
    public void Comparar_DeveEliminarJogosDuplicados()
    {
        var jogosA = CriarColecao(
            CriarJogo(1),
            CriarJogo(1),
            CriarJogo(2));

        var jogosB = CriarColecao(
            CriarJogo(1),
            CriarJogo(1));

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogosA,
            "EstrategiaB",
            jogosB);

        Assert.Equal(2, resultado.QuantidadeJogosA);
        Assert.Equal(1, resultado.QuantidadeJogosB);
        Assert.Equal(1, resultado.QuantidadeJogosComuns);
        Assert.Equal(1, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ComColecoesVazias_DeveRetornarZero()
    {
        var jogosA = CriarColecao();
        var jogosB = CriarColecao();

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogosA,
            "EstrategiaB",
            jogosB);

        Assert.Equal(0, resultado.QuantidadeJogosA);
        Assert.Equal(0, resultado.QuantidadeJogosB);
        Assert.Equal(0, resultado.QuantidadeJogosComuns);
    }

    [Fact]
    public void Comparar_ComApenasUmaColecaoVazia_DeveCalcularExclusivos()
    {
        var jogosA = CriarColecao();
        var jogosB = CriarColecao(CriarJogo(1));

        var resultado = _service.Comparar(
            "EstrategiaA",
            jogosA,
            "EstrategiaB",
            jogosB);

        Assert.Equal(0, resultado.QuantidadeJogosComuns);
        Assert.Equal(0, resultado.QuantidadeJogosExclusivosA);
        Assert.Equal(1, resultado.QuantidadeJogosExclusivosB);
    }

    [Fact]
    public void Comparar_ComColecaoANula_DeveRejeitar()
    {
        var jogosB = CriarColecao(CriarJogo(1));

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                "EstrategiaA",
                null!,
                "EstrategiaB",
                jogosB));
    }

    [Fact]
    public void Comparar_ComColecaoBNula_DeveRejeitar()
    {
        var jogosA = CriarColecao(CriarJogo(1));

        Assert.Throws<ArgumentNullException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogosA,
                "EstrategiaB",
                null!));
    }

    [Fact]
    public void Comparar_ComJogoNulo_DeveRejeitar()
    {
        var jogosA = CriarColecao(new int[][] { null! });

        var jogosB = CriarColecao(CriarJogo(1));

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogosA,
                "EstrategiaB",
                jogosB));
    }

    [Fact]
    public void Comparar_ComJogoInvalido_DeveRejeitar()
    {
        var jogosA = CriarColecao(
            Enumerable.Range(1, 14).ToArray());

        var jogosB = CriarColecao(CriarJogo(1));

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogosA,
                "EstrategiaB",
                jogosB));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Comparar_ComNomeEstrategiaAInvalido_DeveRejeitar(
        string? nome)
    {
        var jogos = CriarColecao(CriarJogo(1));

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                nome!,
                jogos,
                "EstrategiaB",
                jogos));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Comparar_ComNomeEstrategiaBInvalido_DeveRejeitar(
        string? nome)
    {
        var jogos = CriarColecao(CriarJogo(1));

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                "EstrategiaA",
                jogos,
                nome!,
                jogos));
    }
}