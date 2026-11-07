
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;
using Xunit;

namespace LotofacilAnalytics.Backtesting.Tests.Services;

public class ComparacaoPeriodosServiceTests
{
    private readonly ComparacaoPeriodosService _service = new();

    [Fact]
    public void Deve_comparar_uma_estrategia_em_dois_periodos()
    {
        var historico = CriarHistorico();

        var periodos = new[]
        {
            (Inicio: 2, Fim: 3),
            (Inicio: 4, Fim: 5)
        };

        var estrategias = new (string Nome, IEstrategia Estrategia)[]
        {
            ("Fixa", new EstrategiaTeste())
        };

        var resultados = _service.Comparar(
            historico, periodos, estrategias).ToList();

        Assert.Equal(2, resultados.Count);
        Assert.Equal(2, resultados[0].QuantidadeJogos);
        Assert.Equal(2, resultados[1].QuantidadeJogos);

        Assert.Equal(2, resultados[0].NumeroInicial);
        Assert.Equal(5, resultados[1].NumeroFinal);
    }

    [Fact]
    public void Deve_comparar_duas_estrategias_em_dois_periodos()
    {
        var historico = CriarHistorico();

        var periodos = new[]
        {
            (Inicio: 2, Fim: 3),
            (Inicio: 4, Fim: 5)
        };

        var estrategias = new (string Nome, IEstrategia Estrategia)[]
        {
            ("Fixa A", new EstrategiaTeste()),
            ("Fixa B", new EstrategiaTeste())
        };

        var resultados = _service.Comparar(
            historico, periodos, estrategias);

        Assert.Equal(4, resultados.Count);
        Assert.All(resultados, resultado =>
            Assert.Equal(2, resultado.QuantidadeJogos));
    }

    [Fact]
    public void Deve_avaliar_apenas_concursos_do_periodo()
    {
        var historico = CriarHistorico();

        var resultados = _service.Comparar(
            historico,
            new[] { (Inicio: 3, Fim: 5) },
            new (string Nome, IEstrategia Estrategia)[]
            {
                ("Fixa", new EstrategiaTeste())
            });

        var resultado = Assert.Single(resultados);

        Assert.Equal(3, resultado.Resultado.QuantidadeRodadas);
        Assert.Equal(3, resultado.QuantidadeJogos);

        var numerosAvaliados = resultado.Resultado.Rodadas
            .Select(rodada => rodada.ConcursoAlvo.Numero)
            .OrderBy(numero => numero)
            .ToArray();

        Assert.Equal(new[] { 3, 4, 5 }, numerosAvaliados);
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(5, 3)]
    [InlineData(-1, 2)]
    public void Deve_rejeitar_periodos_invalidos(
        int inicio, int fim)
    {
        var historico = CriarHistorico();

        Assert.Throws<ArgumentException>(() =>
            _service.Comparar(
                historico,
                new[] { (Inicio: inicio, Fim: fim) },
                new (string Nome, IEstrategia Estrategia)[]
                {
                    ("Fixa", new EstrategiaTeste())
                }));
    }

    
    [Fact]
    public void Nao_deve_fornecer_concursos_futuros_a_estrategia()
    {
        var historico = CriarHistorico();

        var estrategia = new EstrategiaVerificadora();

        _service.Comparar(
            historico,
            new[] { (Inicio: 3, Fim: 5) },
            new (string Nome, IEstrategia Estrategia)[]
            {
                ("Verificadora", estrategia)
            });

        Assert.Equal(3, estrategia.HistoricosRecebidos.Count);

        Assert.Equal(
            new[] { 1, 2 },
            estrategia.HistoricosRecebidos[0]);

        Assert.Equal(
            new[] { 1, 2, 3 },
            estrategia.HistoricosRecebidos[1]);

        Assert.Equal(
            new[] { 1, 2, 3, 4 },
            estrategia.HistoricosRecebidos[2]);
    }

    [Fact]
    public void Deve_retornar_resultado_vazio_para_periodo_sem_concursos()
    {
        var resultados = _service.Comparar(
            CriarHistorico(),
            new[] { (Inicio: 100, Fim: 110) },
            new (string Nome, IEstrategia Estrategia)[]
            {
                ("Fixa", new EstrategiaTeste())
            });

        var resultado = Assert.Single(resultados);

        Assert.Equal(0, resultado.QuantidadeJogos);
        Assert.Equal(0, resultado.Resultado.QuantidadeRodadas);
    }

    private static List<Concurso> CriarHistorico()
    {
        return Enumerable.Range(1, 6)
            .Select(numero => new Concurso(
                numero,
                new DateTime(2026, 1, 1).AddDays(numero),
                Enumerable.Range(1, 15)))
            .ToList();
    }

    private class EstrategiaTeste : IEstrategia
    {
        public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
            IReadOnlyCollection<Concurso> historico)
        {
            return new List<IReadOnlyCollection<int>>
            {
                Enumerable.Range(1, 15).ToArray()
            };
        }
    }

    private class EstrategiaVerificadora : IEstrategia
    {
        public List<int[]> HistoricosRecebidos { get; } = new();

        public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
            IReadOnlyCollection<Concurso> historico)
        {
            HistoricosRecebidos.Add(
                historico
                    .Select(concurso => concurso.Numero)
                    .ToArray());

            return new List<IReadOnlyCollection<int>>
            {
                Enumerable.Range(1, 15).ToArray()
            };
        }
    }
}