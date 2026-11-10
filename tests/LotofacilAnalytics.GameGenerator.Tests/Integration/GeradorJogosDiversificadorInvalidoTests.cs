
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.GameGenerator.Diversification;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Services;
using LotofacilAnalytics.GameGenerator.Tests.Fixtures;

namespace LotofacilAnalytics.GameGenerator.Tests.Integration;

public class GeradorJogosDiversificadorInvalidoTests
{
    private static GeracaoJogosRequest CriarRequest()
    {
        return new GeracaoJogosRequest(
            quantidadeJogos: 3,
            diversificarJogos: true);
    }

    private static void ExecutarComDiversificador(
        IDiversificadorJogos diversificador)
    {
        var gerador = new GeradorJogosService(diversificador);

        gerador.Gerar(
            ConcursoFixture.CriarHistorico(),
            new EstrategiaFrequenciaDezenas(),
            CriarRequest());
    }

    [Fact]
    public void Gerar_DeveRejeitarQuantidadeIncorreta()
    {
        var diversificador = new DiversificadorSimulado(
            (jogoBase, quantidade, semente) =>
                new[] { jogoBase });

        Assert.Throws<InvalidOperationException>(() =>
            ExecutarComDiversificador(diversificador));
    }

    [Fact]
    public void Gerar_DeveRejeitarJogosDuplicados()
    {
        var diversificador = new DiversificadorSimulado(
            (jogoBase, quantidade, semente) =>
                Enumerable.Repeat(jogoBase, quantidade).ToArray());

        Assert.Throws<InvalidOperationException>(() =>
            ExecutarComDiversificador(diversificador));
    }

    [Fact]
    public void Gerar_DeveRejeitarJogoComQuantidadeInvalidaDeDezenas()
    {
        var diversificador = new DiversificadorSimulado(
            (jogoBase, quantidade, semente) =>
                Enumerable.Range(0, quantidade)
                    .Select(_ =>
                        (IReadOnlyCollection<int>)Enumerable
                            .Range(1, 14)
                            .ToArray())
                    .ToArray());

        Assert.Throws<ArgumentException>(() =>
            ExecutarComDiversificador(diversificador));
    }

    [Fact]
    public void Gerar_DeveRejeitarJogoComDezenasDuplicadas()
    {
        var diversificador = new DiversificadorSimulado(
            (jogoBase, quantidade, semente) =>
                Enumerable.Range(0, quantidade)
                    .Select(_ =>
                        (IReadOnlyCollection<int>)Enumerable
                            .Repeat(1, 15)
                            .ToArray())
                    .ToArray());

        Assert.Throws<ArgumentException>(() =>
            ExecutarComDiversificador(diversificador));
    }

    [Fact]
    public void Gerar_DeveRejeitarJogoComDezenaForaDoIntervalo()
    {
        var diversificador = new DiversificadorSimulado(
            (jogoBase, quantidade, semente) =>
                Enumerable.Range(0, quantidade)
                    .Select(_ =>
                        (IReadOnlyCollection<int>)Enumerable
                            .Range(1, 14)
                            .Append(26)
                            .ToArray())
                    .ToArray());

        Assert.Throws<ArgumentException>(() =>
            ExecutarComDiversificador(diversificador));
    }

    private sealed class DiversificadorSimulado : IDiversificadorJogos
    {
        private readonly Func<
            IReadOnlyCollection<int>,
            int,
            int,
            IReadOnlyCollection<IReadOnlyCollection<int>>> _gerar;

        public DiversificadorSimulado(
            Func<
                IReadOnlyCollection<int>,
                int,
                int,
                IReadOnlyCollection<IReadOnlyCollection<int>>> gerar)
        {
            _gerar = gerar;
        }

        public IReadOnlyCollection<IReadOnlyCollection<int>> Diversificar(
            IReadOnlyCollection<int> jogoBase,
            int quantidadeJogos,
            int semente)
        {
            return _gerar(jogoBase, quantidadeJogos, semente);
        }
    }
}