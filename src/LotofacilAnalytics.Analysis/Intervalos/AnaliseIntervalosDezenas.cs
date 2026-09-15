using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Intervalos;

public class AnaliseIntervalosDezenas
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<int, int>();

        foreach (var concurso in concursos)
        {
            var dezenas = concurso.Dezenas
                .Order()
                .ToArray();

            for (var i = 1; i < dezenas.Length; i++)
            {
                var intervalo =
                    dezenas[i] - dezenas[i - 1];

                if (resultado.ContainsKey(intervalo))
                {
                    resultado[intervalo]++;
                }
                else
                {
                    resultado[intervalo] = 1;
                }
            }
        }

        return resultado;
    }
}