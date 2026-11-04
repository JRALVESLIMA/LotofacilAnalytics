
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Strategies;

public class EstrategiaAleatoria : IEstrategia
{
    private readonly Random _random;

    public EstrategiaAleatoria(int semente = 12345)
    {
        _random = new Random(semente);
    }

    
    public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
        IReadOnlyCollection<Concurso> historico)
    {
        ArgumentNullException.ThrowIfNull(historico);

        if (historico.Count == 0)
        {
            throw new ArgumentException(
                "O histórico não pode estar vazio.",
                nameof(historico));
        }

        var dezenas = Enumerable.Range(1, 25).ToArray();

        for (var i = dezenas.Length - 1; i > 0; i--)
        {
            var indiceAleatorio = _random.Next(i + 1);

            (dezenas[i], dezenas[indiceAleatorio]) =
                (dezenas[indiceAleatorio], dezenas[i]);
        }

        var jogo = dezenas
            .Take(15)
            .OrderBy(dezena => dezena)
            .ToArray();

        IReadOnlyCollection<int> jogoGerado = jogo;

        return new IReadOnlyCollection<int>[] { jogoGerado };
    }

}