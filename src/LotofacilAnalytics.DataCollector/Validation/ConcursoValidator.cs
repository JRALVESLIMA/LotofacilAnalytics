using LotofacilAnalytics.DataCollector.Models;

namespace LotofacilAnalytics.DataCollector.Validation;

public class ConcursoValidator
{
    public bool Validar(ConcursoCaixa concurso)
    {
        if (concurso.Numero <= 0)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(concurso.DataApuracao))
        {
            return false;
        }

        if (concurso.ListaDezenas is null)
        {
            return false;
        }

        if (concurso.ListaDezenas.Count != 15)
        {
            return false;
        }

        var dezenas = new HashSet<int>();

        foreach (var dezenaTexto in concurso.ListaDezenas)
        {
            if (!int.TryParse(dezenaTexto, out var dezena))
            {
                return false;
            }

            if (dezena < 1 || dezena > 25)
            {
                return false;
            }

            if (!dezenas.Add(dezena))
            {
                return false;
            }
        }

        return true;
    }
}