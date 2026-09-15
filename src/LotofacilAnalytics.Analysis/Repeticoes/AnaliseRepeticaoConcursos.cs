using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Repeticoes;

public class AnaliseRepeticaoConcursos
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var listaConcursos = concursos
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        var resultado = new Dictionary<int, int>();

        if (listaConcursos.Count < 2)
            return resultado;

        for (var i = 1; i < listaConcursos.Count; i++)
        {
            var concursoAnterior = listaConcursos[i - 1];
            var concursoAtual = listaConcursos[i];

            var quantidadeRepetidas =
                concursoAtual.Dezenas
                    .Intersect(concursoAnterior.Dezenas)
                    .Count();

            if (resultado.ContainsKey(quantidadeRepetidas))
            {
                resultado[quantidadeRepetidas]++;
            }
            else
            {
                resultado[quantidadeRepetidas] = 1;
            }
        }

        return resultado;
    }
}
