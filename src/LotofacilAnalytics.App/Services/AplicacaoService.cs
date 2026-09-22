using LotofacilAnalytics.Analysis.Services;
using LotofacilAnalytics.App.Models;
using LotofacilAnalytics.DataCollector.Mappers;
using LotofacilAnalytics.DataCollector.Storage;
using LotofacilAnalytics.DataCollector.Validation;
using LotofacilAnalytics.Domain.Entities;

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