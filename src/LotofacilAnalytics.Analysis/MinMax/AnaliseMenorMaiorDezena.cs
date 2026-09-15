using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.MinMax;

public class AnaliseMenorMaiorDezena
{
    public Dictionary<int, int> CalcularMenores(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<int, int>();

        foreach (var concurso in concursos)
        {
            var menor = concurso.MenorDezena;

            if (resultado.ContainsKey(menor))
            {
                resultado[menor]++;
            }
            else
            {
                resultado[menor] = 1;
            }
        }

        return resultado;
    }

    public Dictionary<int, int> CalcularMaiores(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<int, int>();

        foreach (var concurso in concursos)
        {
            var maior = concurso.MaiorDezena;

            if (resultado.ContainsKey(maior))
            {
                resultado[maior]++;
            }
            else
            {
                resultado[maior] = 1;
            }
        }

        return resultado;
    }
}