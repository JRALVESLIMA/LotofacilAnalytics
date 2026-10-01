namespace LotofacilAnalytics.Backtesting.Models;

public class ResultadoJogo
{
    public IReadOnlyCollection<int> Jogo { get; }

    public int QuantidadeAcertos { get; }

    public ResultadoJogo(
        IEnumerable<int> jogo,
        int quantidadeAcertos)
    {
        ArgumentNullException.ThrowIfNull(jogo);

        var listaJogo = jogo.ToArray();

        if (listaJogo.Length != 15)
            throw new ArgumentException(
                "Um jogo deve conter exatamente 15 dezenas.",
                nameof(jogo));

        if (listaJogo.Any(dezena => dezena < 1 || dezena > 25))
            throw new ArgumentException(
                "As dezenas devem estar entre 1 e 25.",
                nameof(jogo));

        if (listaJogo.Distinct().Count() != 15)
            throw new ArgumentException(
                "Um jogo não pode conter dezenas duplicadas.",
                nameof(jogo));

        if (quantidadeAcertos < 0 || quantidadeAcertos > 15)
            throw new ArgumentException(
                "A quantidade de acertos deve estar entre 0 e 15.",
                nameof(quantidadeAcertos));

        Jogo = listaJogo;
        QuantidadeAcertos = quantidadeAcertos;
    }
}