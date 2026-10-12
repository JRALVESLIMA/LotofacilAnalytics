namespace LotofacilAnalytics.Backtesting.Services;

public class AvaliadorJogo
{
    public int CalcularAcertos(
        IEnumerable<int> jogo,
        IEnumerable<int> resultadoReal)
    {
        ArgumentNullException.ThrowIfNull(jogo);
        ArgumentNullException.ThrowIfNull(resultadoReal);

        var dezenasJogo = jogo.ToArray();
        var dezenasResultado = resultadoReal.ToArray();

        ValidarDezenas(dezenasJogo, nameof(jogo));
        ValidarDezenas(dezenasResultado, nameof(resultadoReal));

        return dezenasJogo.Intersect(dezenasResultado).Count();
    }

    private static void ValidarDezenas(
        int[] dezenas,
        string nomeParametro)
    {
        if (dezenas.Length != 15)
        {
            throw new ArgumentException(
                "Devem existir exatamente 15 dezenas.",
                nomeParametro);
        }

        if (dezenas.Any(dezena => dezena < 1 || dezena > 25))
        {
            throw new ArgumentException(
                "As dezenas devem estar entre 1 e 25.",
                nomeParametro);
        }

        if (dezenas.Distinct().Count() != 15)
        {
            throw new ArgumentException(
                "Não são permitidas dezenas duplicadas.",
                nomeParametro);
        }
    }
}
