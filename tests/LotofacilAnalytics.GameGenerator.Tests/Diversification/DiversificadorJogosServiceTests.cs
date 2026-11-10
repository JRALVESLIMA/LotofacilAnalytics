
using LotofacilAnalytics.GameGenerator.Diversification;

namespace LotofacilAnalytics.GameGenerator.Tests.Diversification;

public class DiversificadorJogosServiceTests
{
    private readonly DiversificadorJogosService _service = new();

    private static int[] CriarJogoBase()
    {
        return Enumerable.Range(1, 15).ToArray();
    }

    [Fact]
    public void Diversificar_DevePreservarJogoBaseComoPrimeiro()
    {
        var jogoBase = CriarJogoBase();

        var resultado = _service.Diversificar(jogoBase, 5, 12345);

        Assert.Equal(jogoBase, resultado.First());
    }

    [Fact]
    public void Diversificar_DeveGerarQuantidadeSolicitada()
    {
        var resultado = _service.Diversificar(
            CriarJogoBase(), 10, 12345);

        Assert.Equal(10, resultado.Count);
    }

    [Fact]
    public void Diversificar_DeveGerarJogosValidos()
    {
        var resultado = _service.Diversificar(
            CriarJogoBase(), 20, 12345);

        Assert.All(resultado, jogo =>
        {
            Assert.Equal(15, jogo.Count);
            Assert.Equal(15, jogo.Distinct().Count());

            Assert.All(jogo, dezena =>
                Assert.InRange(dezena, 1, 25));
        });
    }

    [Fact]
    public void Diversificar_NaoDeveGerarJogosDuplicados()
    {
        var resultado = _service.Diversificar(
            CriarJogoBase(), 100, 12345);

        var chaves = resultado
            .Select(jogo => string.Join(",", jogo.Order()))
            .ToArray();

        Assert.Equal(chaves.Length, chaves.Distinct().Count());
    }

    [Fact]
    public void Diversificar_DevePreservarQuatorzeDezenasDoJogoBase()
    {
        var jogoBase = CriarJogoBase();

        var resultado = _service.Diversificar(
            jogoBase, 20, 12345);

        foreach (var jogo in resultado.Skip(1))
        {
            var dezenasEmComum = jogo.Intersect(jogoBase).Count();

            Assert.Equal(14, dezenasEmComum);
        }
    }

    [Fact]
    public void Diversificar_ComMesmaSemente_DeveGerarMesmoResultado()
    {
        var primeiro = _service.Diversificar(
            CriarJogoBase(), 20, 12345);

        var segundo = _service.Diversificar(
            CriarJogoBase(), 20, 12345);

        var chavesPrimeiro = primeiro
            .Select(jogo => string.Join(",", jogo));

        var chavesSegundo = segundo
            .Select(jogo => string.Join(",", jogo));

        Assert.Equal(chavesPrimeiro, chavesSegundo);
    }

    [Fact]
    public void Diversificar_NaoDeveModificarJogoBaseOriginal()
    {
        var jogoBase = CriarJogoBase();
        var copiaOriginal = jogoBase.ToArray();

        _service.Diversificar(jogoBase, 20, 12345);

        Assert.Equal(copiaOriginal, jogoBase);
    }

    [Fact]
    public void Diversificar_DeveRejeitarJogoBaseNulo()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _service.Diversificar(null!, 5, 12345));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Diversificar_DeveRejeitarQuantidadeInvalida(
        int quantidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _service.Diversificar(
                CriarJogoBase(), quantidade, 12345));
    }

    [Fact]
    public void Diversificar_DeveRejeitarJogoBaseInvalido()
    {
        var jogoInvalido = Enumerable.Range(1, 14).ToArray();

        Assert.Throws<ArgumentException>(() =>
            _service.Diversificar(jogoInvalido, 5, 12345));
    }

    [Fact]
    public void Diversificar_NaoDevePermitirAlterarDezenasGeradas()
    {
        var resultado = _service.Diversificar(
            CriarJogoBase(), 5, 12345);

        var jogo = Assert.IsAssignableFrom<IList<int>>(
            resultado.First());

        Assert.Throws<NotSupportedException>(() =>
            jogo[0] = 25);
    }

    [Fact]
    public void Diversificar_NaoDevePermitirAdicionarJogos()
    {
        var resultado = _service.Diversificar(
            CriarJogoBase(), 5, 12345);

        var jogos = Assert.IsAssignableFrom<
            IList<IReadOnlyCollection<int>>>(resultado);

        Assert.Throws<NotSupportedException>(() =>
            jogos.Add(CriarJogoBase()));
    }
}