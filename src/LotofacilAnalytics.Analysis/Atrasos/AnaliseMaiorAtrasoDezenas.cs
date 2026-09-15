using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Atrasos;

public class AnaliseMaiorAtrasoDezenas
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var listaConcursos = concursos
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        var resultado = new Dictionary<int, int>();

        for (var dezena = 1; dezena <= 25; dezena++)
        {
            var maiorAtraso = 0;
            var ultimoConcurso = -1;

            foreach (var concurso in listaConcursos)
            {
                if (!concurso.Dezenas.Contains(dezena))
                {
                    continue;
                }

                if (ultimoConcurso != -1)
                {
                    var atraso = concurso.Numero - ultimoConcurso - 1;

                    if (atraso > maiorAtraso)
                    {
                        maiorAtraso = atraso;
                    }
                }

                ultimoConcurso = concurso.Numero;
            }

            resultado[dezena] = maiorAtraso;
        }

        return resultado;
    }
}