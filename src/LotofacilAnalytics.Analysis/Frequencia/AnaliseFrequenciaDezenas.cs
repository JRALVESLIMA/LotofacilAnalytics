using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Frequencia;

public class AnaliseFrequenciaDezenas
{
    public Dictionary<int, int> CalcularFrequencia(
        IEnumerable<Concurso> concursos)
    {
        var contadores = new Dictionary<int, int>();

        for (int i = 1; i <= 25; i++)
        {
            contadores.Add(i, 0);
        }

        foreach (var concurso in concursos)
        {
            foreach (var dezena in concurso.Dezenas)
            {
                contadores[dezena]++;
            }
        }

        return contadores;
    }
}