using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.ParesImpares;

public class AnaliseParesImpares
{
    public Dictionary<string, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<string, int>();

        foreach (var concurso in concursos)
        {
            var pares = concurso.QuantidadeDezenasPares;
            var impares = concurso.QuantidadeDezenasImpares;

            var chave = $"{pares} pares / {impares} ímpares";

            if (resultado.ContainsKey(chave))
            {
                resultado[chave]++;
            }
            else
            {
                resultado[chave] = 1;
            }
        }

        return resultado;
    }
}