using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Sequencias;

public class AnaliseDistribuicaoSequencias
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<int, int>();

        var analiseSequencias = new AnaliseSequenciasDezenas();

        foreach (var concurso in concursos)
        {
            var maiorSequencia =
                analiseSequencias.CalcularMaiorSequencia(concurso);

            if (resultado.ContainsKey(maiorSequencia))
            {
                resultado[maiorSequencia]++;
            }
            else
            {
                resultado[maiorSequencia] = 1;
            }
        }

        return resultado;
    }
}