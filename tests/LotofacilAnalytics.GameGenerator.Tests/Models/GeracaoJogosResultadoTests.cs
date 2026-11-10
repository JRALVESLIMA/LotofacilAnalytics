
using LotofacilAnalytics.GameGenerator.Models;

namespace LotofacilAnalytics.GameGenerator.Tests.Models;

public class GeracaoJogosResultadoTests
{
    [Fact]
    public void Construtor_DeveArmazenarQuantidadesCorretamente()
    {
        var jogos = new[]
        {
            Enumerable.Range(1, 15).ToArray(),
            Enumerable.Range(2, 15).ToArray()
        };

        var resultado = new GeracaoJogosResultado(5, jogos);

        Assert.Equal(5, resultado.QuantidadeSolicitada);
        Assert.Equal(2, resultado.QuantidadeGerada);
        Assert.Equal(2, resultado.Jogos.Count);
    }

    [Fact]
    public void Construtor_DeveRejeitarQuantidadeGeradaSuperiorASolicitada()
    {
        var jogos = new[]
        {
            Enumerable.Range(1, 15).ToArray(),
            Enumerable.Range(2, 15).ToArray()
        };

        Assert.Throws<ArgumentException>(() =>
            new GeracaoJogosResultado(1, jogos));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Construtor_DeveRejeitarQuantidadeSolicitadaInvalida(
        int quantidade)
    {
        var jogos = Array.Empty<int[]>();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GeracaoJogosResultado(quantidade, jogos));
    }

    [Fact]
    public void Construtor_DeveRejeitarColecaoDeJogosNula()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GeracaoJogosResultado(
                1,
                null!));
    }

    [Fact]
    public void Construtor_DeveRejeitarJogoNulo()
    {
        IEnumerable<int>[] jogos = { null! };

        Assert.Throws<ArgumentNullException>(() =>
            new GeracaoJogosResultado(1, jogos));
    }

    [Fact]
    public void Construtor_DeveProtegerContraAlteracaoDoArrayOriginal()
    {
        var jogoOriginal = Enumerable.Range(1, 15).ToArray();

        var resultado = new GeracaoJogosResultado(
            1,
            new[] { jogoOriginal });

        jogoOriginal[0] = 25;

        Assert.Equal(1, resultado.Jogos.Single().First());
    }

    [Fact]
    public void Jogos_NaoDevePermitirAlterarDezenas()
    {
        var resultado = new GeracaoJogosResultado(
            1,
            new[] { Enumerable.Range(1, 15).ToArray() });

        var dezenas = Assert.IsAssignableFrom<IList<int>>(
            resultado.Jogos.Single());

        Assert.Throws<NotSupportedException>(() =>
            dezenas[0] = 25);
    }

    [Fact]
    public void Jogos_NaoDevePermitirAdicionarJogos()
    {
        var resultado = new GeracaoJogosResultado(
            1,
            new[] { Enumerable.Range(1, 15).ToArray() });

        var jogos = Assert.IsAssignableFrom<
            IList<IReadOnlyCollection<int>>>(resultado.Jogos);

        Assert.Throws<NotSupportedException>(() =>
            jogos.Add(Enumerable.Range(1, 15).ToArray()));
    }
}