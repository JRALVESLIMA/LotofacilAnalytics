
using LotofacilAnalytics.GameGenerator.Validators;

namespace LotofacilAnalytics.GameGenerator.Diversification;

public sealed class DiversificadorJogosService : IDiversificadorJogos
{
    private readonly ValidadorJogo _validador = new();

    public IReadOnlyCollection<IReadOnlyCollection<int>> Diversificar(
        IReadOnlyCollection<int> jogoBase,
        int quantidadeJogos,
        int semente)
    {
        ArgumentNullException.ThrowIfNull(jogoBase);

        _validador.Validar(jogoBase);

        if (quantidadeJogos <= 0 ||
            quantidadeJogos > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeJogos),
                "A quantidade deve estar entre 1 e 100.");
        }

        var dezenasBase = jogoBase.Order().ToArray();

        var dezenasDisponiveis = Enumerable
            .Range(1, 25)
            .Except(dezenasBase)
            .ToArray();

        var combinacoes = new List<int[]>
        {
            dezenasBase
        };

        var variacoesPossiveis = (
            from dezenaRemovida in dezenasBase
            from dezenaAdicionada in dezenasDisponiveis
            select new
            {
                Removida = dezenaRemovida,
                Adicionada = dezenaAdicionada
            })
            .ToList();

        var random = new Random(semente);

        for (int i = variacoesPossiveis.Count - 1; i > 0; i--)
        {
            int indice = random.Next(i + 1);

            (
                variacoesPossiveis[i],
                variacoesPossiveis[indice]
            ) = (
                variacoesPossiveis[indice],
                variacoesPossiveis[i]
            );
        }

        foreach (var variacao in variacoesPossiveis)
        {
            if (combinacoes.Count == quantidadeJogos)
                break;

            var novoJogo = dezenasBase
                .Where(dezena => dezena != variacao.Removida)
                .Append(variacao.Adicionada)
                .Order()
                .ToArray();

            _validador.Validar(novoJogo);

            combinacoes.Add(novoJogo);
        }

        IReadOnlyCollection<int>[] jogos = combinacoes
            .Select(jogo =>
                (IReadOnlyCollection<int>)Array.AsReadOnly(jogo))
            .ToArray();

        return Array.AsReadOnly(jogos);
    }
}