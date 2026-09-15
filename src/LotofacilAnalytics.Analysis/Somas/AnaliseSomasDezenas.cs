using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Somas;

public class AnaliseSomasDezenas
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<int, int>();

        foreach (var concurso in concursos)
        {
            var soma = concurso.SomaDezenas;

            if (resultado.ContainsKey(soma))
            {
                resultado[soma]++;
            }
            else
            {
                resultado[soma] = 1;
            }
        }

        return resultado;
    }
}