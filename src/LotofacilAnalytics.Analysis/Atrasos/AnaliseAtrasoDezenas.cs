using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Atrasos;

public class AnaliseAtrasoDezenas
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var listaConcursos = concursos
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        var resultado = new Dictionary<int, int>();

        if (listaConcursos.Count == 0)
        {
            return resultado;
        }

        var ultimoConcurso = listaConcursos[^1].Numero;

        for (var dezena = 1; dezena <= 25; dezena++)
        {
            var ultimoConcursoDaDezena = listaConcursos
                .LastOrDefault(concurso =>
                    concurso.Dezenas.Contains(dezena));

            if (ultimoConcursoDaDezena is null)
            {
                resultado[dezena] = ultimoConcurso;
                continue;
            }

            resultado[dezena] =
                ultimoConcurso - ultimoConcursoDaDezena.Numero;
        }

        return resultado;
    }
}