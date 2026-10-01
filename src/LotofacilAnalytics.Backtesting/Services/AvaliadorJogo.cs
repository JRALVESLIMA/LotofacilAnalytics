namespace LotofacilAnalytics.Backtesting.Services;

public class AvaliadorJogo
{
    public int CalcularAcertos(
        IEnumerable<int> jogo,
        IEnumerable<int> resultadoReal)
    {
        return jogo.Intersect(resultadoReal).Count();
    }
}