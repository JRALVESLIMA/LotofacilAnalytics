namespace LotofacilAnalytics.GameGenerator.Models;

public sealed class GeracaoJogosResultado
{
    public int QuantidadeSolicitada { get; }

    public int QuantidadeGerada => Jogos.Count;

    public IReadOnlyCollection<IReadOnlyCollection<int>> Jogos { get; }

    public GeracaoJogosResultado(
        int quantidadeSolicitada,
        IEnumerable<IEnumerable<int>> jogos)
    {
        ArgumentNullException.ThrowIfNull(jogos);

        if (quantidadeSolicitada <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeSolicitada),
                "A quantidade solicitada deve ser maior que zero.");
        }

        var jogosMaterializados = jogos
            .Select(jogo =>
            {
                ArgumentNullException.ThrowIfNull(jogo);

                IReadOnlyCollection<int> dezenas =
                    Array.AsReadOnly(jogo.ToArray());

                return dezenas;
            })
            .ToArray();

        if (jogosMaterializados.Length > quantidadeSolicitada)
        {
            throw new ArgumentException(
                "A quantidade gerada não pode superar a solicitada.",
                nameof(jogos));
        }

        QuantidadeSolicitada = quantidadeSolicitada;

        Jogos = Array.AsReadOnly(jogosMaterializados);
    }
}