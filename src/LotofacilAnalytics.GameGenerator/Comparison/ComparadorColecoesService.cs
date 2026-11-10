
using LotofacilAnalytics.GameGenerator.Validators;

namespace LotofacilAnalytics.GameGenerator.Comparison;

public sealed class ComparadorColecoesService
{
    private readonly ValidadorJogo _validador = new();

    public ComparacaoColecoesResultado Comparar(
        string nomeEstrategiaA,
        IReadOnlyCollection<IReadOnlyCollection<int>> jogosA,
        string nomeEstrategiaB,
        IReadOnlyCollection<IReadOnlyCollection<int>> jogosB)
    {
        ArgumentNullException.ThrowIfNull(jogosA);
        ArgumentNullException.ThrowIfNull(jogosB);

        if (string.IsNullOrWhiteSpace(nomeEstrategiaA))
        {
            throw new ArgumentException(
                "O nome da estratégia A é obrigatório.",
                nameof(nomeEstrategiaA));
        }

        if (string.IsNullOrWhiteSpace(nomeEstrategiaB))
        {
            throw new ArgumentException(
                "O nome da estratégia B é obrigatório.",
                nameof(nomeEstrategiaB));
        }

        var combinacoesA = NormalizarJogos(jogosA);
        var combinacoesB = NormalizarJogos(jogosB);

        int quantidadeComuns = combinacoesA
            .Intersect(combinacoesB)
            .Count();

        return new ComparacaoColecoesResultado(
            nomeEstrategiaA,
            nomeEstrategiaB,
            combinacoesA.Count,
            combinacoesB.Count,
            quantidadeComuns);
    }

    private HashSet<string> NormalizarJogos(
        IReadOnlyCollection<IReadOnlyCollection<int>> jogos)
    {
        var combinacoes = new HashSet<string>();

        foreach (var jogo in jogos)
        {
            if (jogo is null)
            {
                throw new ArgumentException(
                    "A coleção não pode conter jogos nulos.",
                    nameof(jogos));
            }

            _validador.Validar(jogo);

            var jogoOrdenado = jogo.Order().ToArray();

            string chave = string.Join(",", jogoOrdenado);

            combinacoes.Add(chave);
        }

        return combinacoes;
    }
}