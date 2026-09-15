using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Frequencia;

public class AnaliseFrequenciaPorAno
{
    public Dictionary<int, Dictionary<int, int>> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado =
            new Dictionary<int, Dictionary<int, int>>();

        foreach (var concurso in concursos)
        {
            var ano = concurso.DataSorteio.Year;

            if (!resultado.ContainsKey(ano))
            {
                resultado[ano] = new Dictionary<int, int>();

                for (var dezena = 1; dezena <= 25; dezena++)
                {
                    resultado[ano][dezena] = 0;
                }
            }

            foreach (var dezena in concurso.Dezenas)
            {
                resultado[ano][dezena]++;
            }
        }

        return resultado;
    }
}