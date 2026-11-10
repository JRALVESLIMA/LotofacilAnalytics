
using LotofacilAnalytics.GameGenerator.Validators;

namespace LotofacilAnalytics.GameGenerator.Comparison;

public sealed class ComparadorJogosService
{
    private readonly ValidadorJogo _validador = new();

    public ComparacaoJogosResultado Comparar(
        string nomeEstrategiaA,
        IReadOnlyCollection<int> jogoA,
        string nomeEstrategiaB,
        IReadOnlyCollection<int> jogoB)
    {
        ArgumentNullException.ThrowIfNull(jogoA);
        ArgumentNullException.ThrowIfNull(jogoB);

        _validador.Validar(jogoA);
        _validador.Validar(jogoB);

        int quantidadeDezenasComuns =
            jogoA.Intersect(jogoB).Count();

        return new ComparacaoJogosResultado(
            nomeEstrategiaA,
            nomeEstrategiaB,
            quantidadeDezenasComuns);
    }
}