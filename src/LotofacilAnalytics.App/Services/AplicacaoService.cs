using LotofacilAnalytics.Analysis.Services;
using LotofacilAnalytics.App.Models;
using LotofacilAnalytics.DataCollector.Mappers;
using LotofacilAnalytics.DataCollector.Storage;
using LotofacilAnalytics.DataCollector.Validation;
using LotofacilAnalytics.Domain.Entities;
using LotofacilAnalytics.Backtesting.Models;
using LotofacilAnalytics.Backtesting.Services;
using LotofacilAnalytics.Backtesting.Strategies;

namespace LotofacilAnalytics.App.Services;

public class AplicacaoService
{
    private readonly string _diretorioRaw;

    public AplicacaoService(string diretorioRaw)
    {
        _diretorioRaw = diretorioRaw;
    }

    public async Task<List<Concurso>> CarregarHistoricoAsync()
    {
        var validator = new ConcursoValidator();
        var mapper = new ConcursoMapper();

        var reader = new ConcursoHistoricoReader(
            _diretorioRaw,
            validator,
            mapper);

        return await reader.LerTodosAsync();
    }

    public IReadOnlyCollection<ComparacaoEstrategiaResultado>
        CompararEstrategias(List<Concurso> concursos)
    {
        ArgumentNullException.ThrowIfNull(concursos);

        const int quantidadeConcursosAvaliados = 101;

        var concursosOrdenados = concursos
            .OrderBy(concurso => concurso.Numero)
            .ToList();

        if (concursosOrdenados.Count <= quantidadeConcursosAvaliados)
        {
            throw new ArgumentException(
                "O histórico precisa conter pelo menos 102 concursos.",
                nameof(concursos));
        }

        var numeroInicial = concursosOrdenados[
            ^quantidadeConcursosAvaliados].Numero;

        var numeroFinal = concursosOrdenados[^1].Numero;

        var estrategias = new Dictionary<string, IEstrategia>
        {
            ["Frequencia Geral"] =
                new EstrategiaFrequenciaDezenas(),

            ["Frequencia Recente"] =
                new EstrategiaFrequenciaRecente(50),

            ["Dezenas Fixas"] =
                new EstrategiaDezenasFixas(),

            ["Aleatoria"] =
                new EstrategiaAleatoria(semente: 12345)
        };

        var comparacaoService = new ComparacaoEstrategiasService();

        return comparacaoService.Comparar(
            concursosOrdenados,
            estrategias,
            numeroInicial,
            numeroFinal);
    }

    public RelatorioAnalise ExecutarAnalises(
        List<Concurso> concursos)
    {
        var service = new AnaliseEstatisticaService();

        const int concursosRecentes = 20;

        return new RelatorioAnalise
        {
            Frequencias =
                service.CalcularFrequencia(concursos),

            ParesImpares =
                service.CalcularParesImpares(concursos),

            Somas =
                service.CalcularSomas(concursos),

            Menores =
                service.CalcularMenoresDezenas(concursos),

            Maiores =
                service.CalcularMaioresDezenas(concursos),

            DistribuicaoSomas =
                service.CalcularDistribuicaoSomas(concursos),

            DistribuicaoFaixas =
                service.CalcularDistribuicaoFaixasHistorica(
                    concursos),

            BaixasAltas =
                service.CalcularBaixasAltas(concursos),

            Atrasos =
                service.CalcularAtrasos(concursos),

            FrequenciaRecente =
                service.CalcularFrequenciaRecente(
                    concursos,
                    concursosRecentes),

            ConcursosRecentes =
                concursosRecentes,

            MaioresAtrasos =
                service.CalcularMaiorAtraso(concursos),

            Repeticoes =
                service.CalcularRepeticoes(concursos),

            Coocorrencias =
                service.CalcularCoocorrencia(concursos),

            Sequencias =
                service.CalcularMaioresSequencias(
                    concursos),

            ParesConsecutivos =
                service.CalcularParesConsecutivos(
                    concursos),

            Intervalos =
                service.CalcularIntervalos(
                    concursos),

            DistribuicaoSequencias =
                service.CalcularDistribuicaoSequencias(
                    concursos)
        };
    }
}