using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Sequencias;

public class AnaliseParesConsecutivos
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<int, int>();

        foreach (var concurso in concursos)
        {
            var dezenas = concurso.Dezenas
                .Order()
                .ToList();

            var quantidadePares = 0;

            for (var i = 1; i < dezenas.Count; i++)
            {
                if (dezenas[i] == dezenas[i - 1] + 1)
                {
                    quantidadePares++;
                }
            }

            if (resultado.ContainsKey(quantidadePares))
            {
                resultado[quantidadePares]++;
            }
            else
            {
                resultado[quantidadePares] = 1;
            }
        }

        return resultado;
    }
}