
using FluentAssertions;
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Backtesting.Tests;

public class ComparacaoEstrategiasServiceTests
{
    [Fact]
    public void Deve_comparar_multiplas_estrategias_no_mesmo_periodo()
    {
        var historico = CriarHistorico();

        var estrategias = new Dictionary<string, IEstrategia>
        {
            ["Frequencia Geral"] = new EstrategiaFrequenciaDezenas(),
            ["Dezenas Fixas"] = new EstrategiaDezenasFixas()
        };

        var service = new ComparacaoEstrategiasService();

        var resultados = service.Comparar(
            historico,
            estrategias,
            4,
            7);

        resultados.Should().HaveCount(2);

        resultados
            .Select(resultado => resultado.NomeEstrategia)
            .Should()
            .Equal("Frequencia Geral", "Dezenas Fixas");

        foreach (var resultado in resultados)
        {
            resultado.QuantidadeRodadas.Should().Be(4);
            resultado.QuantidadeJogos.Should().Be(4);

            resultado.Resultado.Rodadas
                .Select(rodada => rodada.ConcursoAlvo.Numero)
                .Should()
                .Equal(4, 5, 6, 7);

            resultado.MediaAcertos.Should().BeInRange(0, 15);
        }
    }

    [Fact]
    public void Deve_preservar_historico_anterior_para_cada_estrategia()
    {
        var historico = CriarHistorico();

        var primeira = new EstrategiaFake();
        var segunda = new EstrategiaFake();

        var estrategias = new Dictionary<string, IEstrategia>
        {
            ["Primeira"] = primeira,
            ["Segunda"] = segunda
        };

        var service = new ComparacaoEstrategiasService();

        service.Comparar(historico, estrategias, 4, 5);

        foreach (var estrategia in new[] { primeira, segunda })
        {
            estrategia.HistoricosRecebidos
                .Select(h => h.Select(c => c.Numero))
                .Should()
                .BeEquivalentTo(
                    new[]
                    {
                        new[] { 1, 2, 3 },
                        new[] { 1, 2, 3, 4 }
                    });
        }
    }

    [Fact]
    public void Deve_rejeitar_historico_nulo()
    {
        var estrategias = new Dictionary<string, IEstrategia>
        {
            ["Teste"] = new EstrategiaDezenasFixas()
        };

        var service = new ComparacaoEstrategiasService();

        Action acao = () => service.Comparar(
            null!,
            estrategias,
            2,
            5);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_colecao_de_estrategias_nula()
    {
        var service = new ComparacaoEstrategiasService();

        Action acao = () => service.Comparar(
            CriarHistorico(),
            null!,
            2,
            5);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deve_rejeitar_colecao_de_estrategias_vazia()
    {
        var service = new ComparacaoEstrategiasService();

        var estrategias = new Dictionary<string, IEstrategia>();

        Action acao = () => service.Comparar(
            CriarHistorico(),
            estrategias,
            2,
            5);

        acao.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_rejeitar_nome_de_estrategia_invalido(
        string nome)
    {
        var estrategias = new Dictionary<string, IEstrategia>
        {
            [nome] = new EstrategiaDezenasFixas()
        };

        var service = new ComparacaoEstrategiasService();

        Action acao = () => service.Comparar(
            CriarHistorico(),
            estrategias,
            2,
            5);

        acao.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Deve_rejeitar_estrategia_nula()
    {
        var estrategias = new Dictionary<string, IEstrategia>
        {
            ["Teste"] = null!
        };

        var service = new ComparacaoEstrategiasService();

        Action acao = () => service.Comparar(
            CriarHistorico(),
            estrategias,
            2,
            5);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(6, 5)]
    public void Deve_rejeitar_periodo_invalido(
        int numeroInicial,
        int numeroFinal)
    {
        var estrategias = new Dictionary<string, IEstrategia>
        {
            ["Teste"] = new EstrategiaDezenasFixas()
        };

        var service = new ComparacaoEstrategiasService();

        Action acao = () => service.Comparar(
            CriarHistorico(),
            estrategias,
            numeroInicial,
            numeroFinal);

        acao.Should().Throw<ArgumentException>();
    }

    private static IReadOnlyCollection<Concurso> CriarHistorico()
    {
        return Enumerable.Range(1, 10)
            .Select(numero => new Concurso(
                numero,
                new DateTime(2026, 1, numero),
                Enumerable.Range(1, 15)))
            .ToArray();
    }

    private sealed class EstrategiaFake : IEstrategia
    {
        private readonly List<IReadOnlyCollection<Concurso>>
            _historicosRecebidos = new();

        public IReadOnlyCollection<IReadOnlyCollection<Concurso>>
            HistoricosRecebidos => _historicosRecebidos;

        public IReadOnlyCollection<IReadOnlyCollection<int>> GerarJogos(
            IReadOnlyCollection<Concurso> historico)
        {
            _historicosRecebidos.Add(historico.ToArray());

            return new[]
            {
                (IReadOnlyCollection<int>)
                    Enumerable.Range(1, 15).ToArray()
            };
        }
    }
}