using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Faixas;

public class AnaliseDistribuicaoFaixas
{
    public Dictionary<string, int> Calcular(
        Concurso concurso)
    {
        var resultado = new Dictionary<string, int>
        {
            ["01-05"] = 0,
            ["06-10"] = 0,
            ["11-15"] = 0,
            ["16-20"] = 0,
            ["21-25"] = 0
        };

        foreach (var dezena in concurso.Dezenas)
        {
            if (dezena <= 5)
            {
                resultado["01-05"]++;
            }
            else if (dezena <= 10)
            {
                resultado["06-10"]++;
            }
            else if (dezena <= 15)
            {
                resultado["11-15"]++;
            }
            else if (dezena <= 20)
            {
                resultado["16-20"]++;
            }
            else
            {
                resultado["21-25"]++;
            }
        }

        return resultado;
    }
}