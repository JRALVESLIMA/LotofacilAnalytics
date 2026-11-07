
namespace LotofacilAnalytics.Backtesting.Services;

public class ProbabilidadeTeoricaService
{
    public double CalcularProbabilidade(int acertos)
    {
        if (acertos < 5 || acertos > 15)
        {
            throw new ArgumentOutOfRangeException(
                nameof(acertos),
                "A quantidade de acertos deve estar entre 5 e 15.");
        }

        double casosFavoraveis =
            CalcularCombinacao(15, acertos) *
            CalcularCombinacao(10, 15 - acertos);

        double casosPossiveis =
            CalcularCombinacao(25, 15);

        return casosFavoraveis / casosPossiveis;
    }

    private static double CalcularCombinacao(int n, int k)
    {
        if (k < 0 || k > n)
            return 0;

        k = Math.Min(k, n - k);

        double resultado = 1;

        for (int i = 1; i <= k; i++)
        {
            resultado *= (n - k + i);
            resultado /= i;
        }

        return resultado;
    }
}