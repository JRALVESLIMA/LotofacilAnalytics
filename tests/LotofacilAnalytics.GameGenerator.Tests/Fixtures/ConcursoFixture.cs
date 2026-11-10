
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.GameGenerator.Tests.Fixtures;

public static class ConcursoFixture
{
    public static IReadOnlyCollection<Concurso> CriarHistorico()
    {
        return new[]
        {
            new Concurso(
                1,
                new DateTime(2026, 1, 1),
                Enumerable.Range(1, 15)),

            new Concurso(
                2,
                new DateTime(2026, 1, 2),
                Enumerable.Range(1, 15)),

            new Concurso(
                3,
                new DateTime(2026, 1, 3),
                Enumerable.Range(11, 15)),

            new Concurso(
                4,
                new DateTime(2026, 1, 4),
                Enumerable.Range(11, 15))
        };
    }
}