
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Services;
using LotofacilAnalytics.GameGenerator.Reports;

namespace LotofacilAnalytics.GameGenerator.Comparison;

public sealed class ComparadorEstrategiasService
{
    private readonly GeradorJogosService _gerador;
    private readonly ComparadorColecoesService _comparador;
    private readonly RelatorioGeracaoJogosService _relatorioService = new();

    public ComparadorEstrategiasService(
        GeradorJogosService? gerador = null,
        ComparadorColecoesService? comparador = null)
    {
        _gerador = gerador ?? new GeradorJogosService();
        _comparador = comparador ?? new ComparadorColecoesService();
    }

    public ComparacaoColecoesResultado Comparar(
        IReadOnlyCollection<Concurso> historico,
        string nomeEstrategiaA,
        IEstrategia estrategiaA,
        GeracaoJogosRequest requestA,
        string nomeEstrategiaB,
        IEstrategia estrategiaB,
        GeracaoJogosRequest requestB)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(estrategiaA);
        ArgumentNullException.ThrowIfNull(estrategiaB);
        ArgumentNullException.ThrowIfNull(requestA);
        ArgumentNullException.ThrowIfNull(requestB);

        if (string.IsNullOrWhiteSpace(nomeEstrategiaA))
        {
            throw new ArgumentException(
                "O nome da estratégia A é obrigatório.",
                nameof(nomeEstrategiaA));
        }

        if (string.IsNullOrWhiteSpace(nomeEstrategiaB))
        {
            throw new ArgumentException(
                "O nome da estratégia B é obrigatório.",
                nameof(nomeEstrategiaB));
        }

        var resultadoA = _gerador.Gerar(
            historico,
            estrategiaA,
            requestA);

        var resultadoB = _gerador.Gerar(
            historico,
            estrategiaB,
            requestB);

        return _comparador.Comparar(
            nomeEstrategiaA,
            resultadoA.Jogos,
            nomeEstrategiaB,
            resultadoB.Jogos);
    }

    public ComparacaoEstrategiasDetalhadaResultado CompararDetalhado(
        IReadOnlyCollection<Concurso> historico,
        string nomeEstrategiaA,
        IEstrategia estrategiaA,
        GeracaoJogosRequest requestA,
        string nomeEstrategiaB,
        IEstrategia estrategiaB,
        GeracaoJogosRequest requestB)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(estrategiaA);
        ArgumentNullException.ThrowIfNull(estrategiaB);
        ArgumentNullException.ThrowIfNull(requestA);
        ArgumentNullException.ThrowIfNull(requestB);

        if (string.IsNullOrWhiteSpace(nomeEstrategiaA))
        {
            throw new ArgumentException(
                "O nome da estratégia A é obrigatório.",
                nameof(nomeEstrategiaA));
        }

        if (string.IsNullOrWhiteSpace(nomeEstrategiaB))
        {
            throw new ArgumentException(
                "O nome da estratégia B é obrigatório.",
                nameof(nomeEstrategiaB));
        }

        var geracaoA = _gerador.Gerar(
            historico,
            estrategiaA,
            requestA);

        var geracaoB = _gerador.Gerar(
            historico,
            estrategiaB,
            requestB);

        var relatorioA = _relatorioService.Criar(
            nomeEstrategiaA,
            requestA,
            geracaoA);

        var relatorioB = _relatorioService.Criar(
            nomeEstrategiaB,
            requestB,
            geracaoB);

        var comparacao = _comparador.Comparar(
            nomeEstrategiaA,
            geracaoA.Jogos,
            nomeEstrategiaB,
            geracaoB.Jogos);

        return new ComparacaoEstrategiasDetalhadaResultado(
            geracaoA,
            geracaoB,
            relatorioA,
            relatorioB,
            comparacao);
    }
}