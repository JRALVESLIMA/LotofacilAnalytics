using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Coocorrencia;

public class AnaliseCoocorrenciaDezenas
{
    public Dictionary<(int Dezena1, int Dezena2), int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado =
            new Dictionary<(int Dezena1, int Dezena2), int>();

        foreach (var concurso in concursos)
        {
            var dezenas = concurso.Dezenas
                .Order()
                .ToArray();

            for (var i = 0; i < dezenas.Length; i++)
            {
                for (var j = i + 1; j < dezenas.Length; j++)
                {
                    var par = (dezenas[i], dezenas[j]);

                    if (resultado.ContainsKey(par))
                    {
                        resultado[par]++;
                    }
                    else
                    {
                        resultado[par] = 1;
                    }
                }
            }
        }

        return resultado;
    }
}