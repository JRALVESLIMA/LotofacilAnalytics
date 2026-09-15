using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Faixas;

public class AnaliseBaixasAltas
{
    public Dictionary<string, int> Calcular(
        IEnumerable<Concurso> concursos)
    {
        var resultado = new Dictionary<string, int>();

        foreach (var concurso in concursos)
        {
            var baixas = concurso.Dezenas
                .Count(dezena => dezena <= 13);

            var altas = concurso.Dezenas
                .Count(dezena => dezena >= 14);

            var chave = $"{baixas} baixas / {altas} altas";

            if (resultado.ContainsKey(chave))
            {
                resultado[chave]++;
            }
            else
            {
                resultado[chave] = 1;
            }
        }

        return resultado;
    }
}