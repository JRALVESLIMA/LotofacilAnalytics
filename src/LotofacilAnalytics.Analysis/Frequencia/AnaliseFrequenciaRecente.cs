using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Frequencia;

public class AnaliseFrequenciaRecente
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos,
        int quantidadeConcursos)
    {
        if (quantidadeConcursos <= 0)
        {
            throw new ArgumentException(
                "A quantidade de concursos deve ser maior que zero.",
                nameof(quantidadeConcursos));
        }

        var listaConcursos = concursos
            .OrderByDescending(concurso => concurso.Numero)
            .Take(quantidadeConcursos)
            .ToList();

        var resultado = new Dictionary<int, int>();

        for (var dezena = 1; dezena <= 25; dezena++)
        {
            resultado[dezena] = 0;
        }

        foreach (var concurso in listaConcursos)
        {
            foreach (var dezena in concurso.Dezenas)
            {
                resultado[dezena]++;
            }
        }

        return resultado;
    }
}