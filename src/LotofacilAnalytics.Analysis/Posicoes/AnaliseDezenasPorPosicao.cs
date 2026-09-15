using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Posicoes;

public class AnaliseDezenasPorPosicao
{
    public Dictionary<int, Dictionary<int, int>> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = CriarResultadoInicial();

        foreach (var concurso in concursos)
        {
            var dezenas = concurso.Dezenas
                .Order()
                .ToArray();

            for (var i = 0; i < dezenas.Length; i++)
            {
                var posicao = i + 1;
                var dezena = dezenas[i];

                resultado[posicao][dezena]++;
            }
        }

        return resultado;
    }

    private static Dictionary<int, Dictionary<int, int>>
        CriarResultadoInicial()
    {
        var resultado =
            new Dictionary<int, Dictionary<int, int>>();

        for (var posicao = 1; posicao <= 15; posicao++)
        {
            resultado[posicao] = new Dictionary<int, int>();

            for (var dezena = 1; dezena <= 25; dezena++)
            {
                resultado[posicao][dezena] = 0;
            }
        }

        return resultado;
    }
}