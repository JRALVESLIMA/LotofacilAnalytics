using LotofacilAnalytics.Analysis.Atrasos;
using LotofacilAnalytics.Analysis.Coocorrencia;
using LotofacilAnalytics.Analysis.Faixas;
using LotofacilAnalytics.Analysis.Frequencia;
using LotofacilAnalytics.Analysis.Intervalos;
using LotofacilAnalytics.Analysis.MinMax;
using LotofacilAnalytics.Analysis.ParesImpares;
using LotofacilAnalytics.Analysis.Posicoes;
using LotofacilAnalytics.Analysis.Repeticoes;
using LotofacilAnalytics.Analysis.Sequencias;
using LotofacilAnalytics.Analysis.Somas;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.Analysis.Services;

public class AnaliseEstatisticaService
{
    public Dictionary<int, int> CalcularFrequencia(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseFrequenciaDezenas();

        return analise.CalcularFrequencia(concursos);
    }

    public Dictionary<string, int> CalcularParesImpares(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseParesImpares();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularSomas(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseSomasDezenas();

        return analise.Calcular(concursos);
    }

    public Dictionary<string, int> CalcularDistribuicaoSomas(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseDistribuicaoSomas();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularMenoresDezenas(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseMenorMaiorDezena();

        return analise.CalcularMenores(concursos);
    }

    public Dictionary<int, int> CalcularMaioresDezenas(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseMenorMaiorDezena();

        return analise.CalcularMaiores(concursos);
    }

    public Dictionary<string, int> CalcularDistribuicaoFaixas(
        Concurso concurso)
    {
        var analise = new AnaliseDistribuicaoFaixas();

        return analise.Calcular(concurso);
    }

    public Dictionary<string, Dictionary<int, int>>
        CalcularDistribuicaoFaixasHistorica(
            IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseDistribuicaoFaixasHistorica();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, Dictionary<int, int>> CalcularFrequenciaPorAno(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseFrequenciaPorAno();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularAtrasos(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseAtrasoDezenas();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularFrequenciaRecente(
        IEnumerable<Concurso> concursos,
        int quantidadeConcursos)
    {
        var analise = new AnaliseFrequenciaRecente();

        return analise.Calcular(
            concursos,
            quantidadeConcursos);
    }

    public Dictionary<int, int> CalcularMaiorAtraso(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseMaiorAtrasoDezenas();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularMaioresSequencias(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseSequenciasDezenas();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularRepeticoes(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseRepeticaoConcursos();

        return analise.Calcular(concursos);
    }

    public Dictionary<(int Dezena1, int Dezena2), int>
        CalcularCoocorrencia(
            IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseCoocorrenciaDezenas();

        return analise.Calcular(concursos);
    }

    public Dictionary<string, int> CalcularBaixasAltas(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseBaixasAltas();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularParesConsecutivos(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseParesConsecutivos();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, Dictionary<int, int>>
        CalcularDezenasPorPosicao(
            IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseDezenasPorPosicao();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularIntervalos(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseIntervalosDezenas();

        return analise.Calcular(concursos);
    }

    public Dictionary<int, int> CalcularDistribuicaoSequencias(
        IEnumerable<Concurso> concursos)
    {
        var analise = new AnaliseDistribuicaoSequencias();

        return analise.Calcular(concursos);
    }
}
