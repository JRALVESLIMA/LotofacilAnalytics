using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Sequencias;

public class AnaliseSequenciasDezenas
{
    public Dictionary<int, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<int, int>();

        foreach (var concurso in concursos)
        {
            var maiorSequencia = CalcularMaiorSequencia(concurso);

            resultado[concurso.Numero] = maiorSequencia;
        }

        return resultado;
    }

    public int CalcularMaiorSequencia(Concurso concurso)
    {
        ArgumentNullException.ThrowIfNull(concurso);

        var dezenas = concurso.Dezenas
            .Order()
            .ToArray();

        var maiorSequencia = 1;
        var sequenciaAtual = 1;

        for (var i = 1; i < dezenas.Length; i++)
        {
            if (dezenas[i] == dezenas[i - 1] + 1)
            {
                sequenciaAtual++;

                if (sequenciaAtual > maiorSequencia)
                {
                    maiorSequencia = sequenciaAtual;
                }
            }
            else
            {
                sequenciaAtual = 1;
            }
        }

        return maiorSequencia;
    }
}