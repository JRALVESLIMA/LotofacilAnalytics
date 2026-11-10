
using LotofacilAnalytics.Backtesting.Strategies;
using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.GameGenerator.Diversification;
using LotofacilAnalytics.GameGenerator.Models;
using LotofacilAnalytics.GameGenerator.Validators;

namespace LotofacilAnalytics.GameGenerator.Services;

public sealed class GeradorJogosService
{
    private readonly ValidadorJogo _validador = new();
    private readonly IDiversificadorJogos _diversificador;

    public GeradorJogosService(
        IDiversificadorJogos? diversificador = null)
    {
        _diversificador = diversificador
            ?? new DiversificadorJogosService();
    }

    public GeracaoJogosResultado Gerar(
        IReadOnlyCollection<Concurso> historico,
        IEstrategia estrategia,
        GeracaoJogosRequest request)
    {
        ArgumentNullException.ThrowIfNull(historico);
        ArgumentNullException.ThrowIfNull(estrategia);
        ArgumentNullException.ThrowIfNull(request);

        if (historico.Count == 0)
        {
            throw new ArgumentException(
                "O histórico não pode estar vazio.",
                nameof(historico));
        }

        if (request.DiversificarJogos)
        {
            return GerarComDiversificacao(
                historico,
                estrategia,
                request);
        }

        return GerarSemDiversificacao(
            historico,
            estrategia,
            request);
    }

    private GeracaoJogosResultado GerarComDiversificacao(
        IReadOnlyCollection<Concurso> historico,
        IEstrategia estrategia,
        GeracaoJogosRequest request)
    {
        var jogosBase = estrategia.GerarJogos(historico);

        if (jogosBase is null || jogosBase.Count == 0)
        {
            throw new InvalidOperationException(
                "A estratégia não retornou jogos.");
        }

        // Política inicial: diversificar apenas o primeiro jogo.
        var jogoBase = jogosBase.First();

        if (jogoBase is null)
        {
            throw new InvalidOperationException(
                "A estratégia retornou um jogo nulo.");
        }

        _validador.Validar(jogoBase);

        var jogosDiversificados = _diversificador.Diversificar(
            jogoBase,
            request.QuantidadeJogos,
            request.SementeDiversificacao);

        if (jogosDiversificados is null ||
            jogosDiversificados.Count != request.QuantidadeJogos)
        {
            throw new InvalidOperationException(
                "O diversificador não retornou a quantidade solicitada.");
        }

        foreach (var jogo in jogosDiversificados)
        {
            _validador.Validar(jogo);
        }

        if (jogosDiversificados
            .Select(jogo => string.Join(",", jogo.Order()))
            .Distinct()
            .Count() != request.QuantidadeJogos)
        {
            throw new InvalidOperationException(
                "O diversificador retornou jogos duplicados.");
        }

        return new GeracaoJogosResultado(
            request.QuantidadeJogos,
            jogosDiversificados);
    }

    private GeracaoJogosResultado GerarSemDiversificacao(
        IReadOnlyCollection<Concurso> historico,
        IEstrategia estrategia,
        GeracaoJogosRequest request)
    {
        var jogos = new List<IReadOnlyCollection<int>>();
        var combinacoesEncontradas = new HashSet<string>();

        const int maximoTentativasPorJogo = 100;

        var limiteTentativas = checked(
            request.QuantidadeJogos * maximoTentativasPorJogo);

        var tentativas = 0;

        while (jogos.Count < request.QuantidadeJogos &&
               tentativas < limiteTentativas)
        {
            tentativas++;

            var jogosCandidatos = estrategia.GerarJogos(historico);

            if (jogosCandidatos is null ||
                jogosCandidatos.Count == 0)
            {
                throw new InvalidOperationException(
                    "A estratégia não retornou jogos.");
            }

            foreach (var candidato in jogosCandidatos)
            {
                if (candidato is null)
                {
                    throw new InvalidOperationException(
                        "A estratégia retornou um jogo nulo.");
                }

                _validador.Validar(candidato);

                var jogoOrdenado = candidato
                    .Order()
                    .ToArray();

                var chave = string.Join(",", jogoOrdenado);

                if (!request.PermitirJogosRepetidos &&
                    !combinacoesEncontradas.Add(chave))
                {
                    continue;
                }

                jogos.Add(jogoOrdenado);

                if (jogos.Count == request.QuantidadeJogos)
                {
                    break;
                }
            }
        }

        if (jogos.Count < request.QuantidadeJogos)
        {
            throw new InvalidOperationException(
                $"Não foi possível gerar {request.QuantidadeJogos} " +
                "jogos com a estratégia informada.");
        }

        return new GeracaoJogosResultado(
            request.QuantidadeJogos,
            jogos);
    }
}