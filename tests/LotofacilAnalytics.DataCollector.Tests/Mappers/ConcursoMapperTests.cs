using LotofacilAnalytics.DataCollector.Mappers;
using LotofacilAnalytics.DataCollector.Models;

namespace LotofacilAnalytics.DataCollector.Tests.Mappers;

public class ConcursoMapperTests
{
    [Fact]
    public void DeveMapearConcursoCaixaParaConcurso()
    {
        var concursoCaixa = new ConcursoCaixa
        {
            Numero = 3779,
            DataApuracao = "03/09/2026",
            ListaDezenas =
            [
                "03", "04", "05", "07", "08",
                "10", "11", "13", "14", "16",
                "17", "19", "23", "24", "25"
            ]
        };

        var mapper = new ConcursoMapper();

        var concurso = mapper.Mapear(concursoCaixa);

        Assert.Equal(3779, concurso.Numero);

        Assert.Equal(
            new DateTime(2026, 9, 3),
            concurso.DataSorteio);

        Assert.Equal(
            [
                3, 4, 5, 7, 8,
                10, 11, 13, 14, 16,
                17, 19, 23, 24, 25
            ],
            concurso.Dezenas);
    }
}