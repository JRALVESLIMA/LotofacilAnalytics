using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Somas;

public class AnaliseDistribuicaoSomas
{
    public Dictionary<string, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<string, int>();

        foreach (var concurso in concursos)
        {
            var faixa = ObterFaixa(concurso.SomaDezenas);

            if (resultado.ContainsKey(faixa))
            {
                resultado[faixa]++;
            }
            else
            {
                resultado[faixa] = 1;
            }
        }

        return resultado;
    }

    private static string ObterFaixa(int soma)
    {
        var inicio = (soma / 10) * 10;
        var fim = inicio + 9;

        return $"{inicio}-{fim}";
    }
}