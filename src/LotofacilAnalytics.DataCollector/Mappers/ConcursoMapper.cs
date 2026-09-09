using System.Globalization;
using LotofacilAnalytics.DataCollector.Models;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.DataCollector.Mappers;

public class ConcursoMapper
{
    public Concurso Mapear(ConcursoCaixa concursoCaixa)
    {
        ArgumentNullException.ThrowIfNull(concursoCaixa);

        var dataSorteio = DateTime.ParseExact(
            concursoCaixa.DataApuracao,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture);

        var dezenas = concursoCaixa.ListaDezenas
            .Select(int.Parse)
            .ToArray();

        return new Concurso(
            concursoCaixa.Numero,
            dataSorteio,
            dezenas);
    }
}