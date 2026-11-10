
namespace LotofacilAnalytics.GameGenerator.Diversification;

public interface IDiversificadorJogos
{
    IReadOnlyCollection<IReadOnlyCollection<int>> Diversificar(
        IReadOnlyCollection<int> jogoBase,
        int quantidadeJogos,
        int semente);
}