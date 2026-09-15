using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Faixas;

public class AnaliseDistribuicaoFaixasHistorica
{
    public Dictionary<string, Dictionary<int, int>> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = CriarResultadoInicial();

        foreach (var concurso in concursos)
        {
            var distribuicao =
                CalcularDistribuicaoDoConcurso(concurso);

            foreach (var item in distribuicao)
            {
                if (resultado[item.Key].ContainsKey(item.Value))
                {
                    resultado[item.Key][item.Value]++;
                }
                else
                {
                    resultado[item.Key][item.Value] = 1;
                }
            }
        }

        return resultado;
    }

    private static Dictionary<string, int>
        CalcularDistribuicaoDoConcurso(Concurso concurso)
    {
        var resultado = new Dictionary<string, int>
        {
            ["01-05"] = 0,
            ["06-10"] = 0,
            ["11-15"] = 0,
            ["16-20"] = 0,
            ["21-25"] = 0
        };

        foreach (var dezena in concurso.Dezenas)
        {
            var faixa = ObterFaixa(dezena);

            resultado[faixa]++;
        }

        return resultado;
    }

    private static string ObterFaixa(int dezena)
    {
        if (dezena <= 5)
            return "01-05";

        if (dezena <= 10)
            return "06-10";

        if (dezena <= 15)
            return "11-15";

        if (dezena <= 20)
            return "16-20";

        return "21-25";
    }

    private static Dictionary<string, Dictionary<int, int>>
        CriarResultadoInicial()
    {
        return new Dictionary<string, Dictionary<int, int>>
        {
            ["01-05"] = [],
            ["06-10"] = [],
            ["11-15"] = [],
            ["16-20"] = [],
            ["21-25"] = []
        };
    }
}