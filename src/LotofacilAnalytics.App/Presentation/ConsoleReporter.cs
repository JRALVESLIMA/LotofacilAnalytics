using LotofacilAnalytics.App.Models;
using LotofacilAnalytics.Domain.Entities;

namespace LotofacilAnalytics.App.Presentation;

public class ConsoleReporter
{
    public void ExibirCabecalho()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("           LOTOFÁCIL ANALYTICS");
        Console.WriteLine("==============================================");
        Console.WriteLine();
    }

    public void ExibirResumoHistorico(
        List<Concurso> concursos)
    {
        Console.WriteLine(
            $"Concursos carregados: {concursos.Count}");

        Console.WriteLine(
            $"Primeiro concurso: {concursos.Min(c => c.Numero)}");

        Console.WriteLine(
            $"Último concurso: {concursos.Max(c => c.Numero)}");

        Console.WriteLine();
    }

    public void ExibirRelatorio(
        RelatorioAnalise relatorio)
    {
        ExibirFrequencias(relatorio.Frequencias);

        ExibirParesImpares(relatorio.ParesImpares);

        ExibirSomas(relatorio.Somas);

        ExibirMenoresEMaiores(
            relatorio.Menores,
            relatorio.Maiores);

        ExibirDistribuicaoSomas(
            relatorio.DistribuicaoSomas);

        ExibirDistribuicaoFaixas(
            relatorio.DistribuicaoFaixas);

        ExibirBaixasAltas(
            relatorio.BaixasAltas);

        ExibirAtrasos(
            relatorio.Atrasos);

        ExibirFrequenciaRecente(
            relatorio.FrequenciaRecente,
            relatorio.ConcursosRecentes);

        ExibirMaiorAtraso(
            relatorio.MaioresAtrasos);

        ExibirRepeticoes(
            relatorio.Repeticoes);

        ExibirCoocorrencias(
            relatorio.Coocorrencias);

        ExibirMaioresSequencias(
            relatorio.Sequencias);

        ExibirParesConsecutivos(
            relatorio.ParesConsecutivos);

        ExibirIntervalos(
            relatorio.Intervalos);

        ExibirDistribuicaoSequencias(
            relatorio.DistribuicaoSequencias);
    }

    public void ExibirFrequencias(
        Dictionary<int, int> frequencias)
    {
        ExibirTitulo("FREQUÊNCIA DAS DEZENAS");

        foreach (var item in frequencias
            .OrderBy(item => item.Key))
        {
            Console.WriteLine(
                $"Dezena {item.Key:D2}: {item.Value} vezes");
        }

        Console.WriteLine();
    }

    public void ExibirParesImpares(
        Dictionary<string, int> paresImpares)
    {
        ExibirTitulo("PARES E ÍMPARES");

        foreach (var item in paresImpares
            .OrderByDescending(item => item.Value))
        {
            Console.WriteLine(
                $"{item.Key}: {item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirSomas(
        Dictionary<int, int> somas)
    {
        ExibirTitulo("SOMA DAS DEZENAS");

        Console.WriteLine(
            $"Menor soma encontrada: {somas.Keys.Min()}");

        Console.WriteLine(
            $"Maior soma encontrada: {somas.Keys.Max()}");

        Console.WriteLine();
    }

    public void ExibirDistribuicaoSomas(
        Dictionary<string, int> distribuicaoSomas)
    {
        ExibirTitulo("DISTRIBUIÇÃO DAS SOMAS");

        foreach (var item in distribuicaoSomas
            .OrderBy(item => item.Key))
        {
            Console.WriteLine(
                $"Soma {item.Key}: {item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirMenoresEMaiores(
        Dictionary<int, int> menores,
        Dictionary<int, int> maiores)
    {
        ExibirTitulo("MENOR E MAIOR DEZENA");

        Console.WriteLine("Menores dezenas mais frequentes:");

        foreach (var item in menores
            .OrderByDescending(item => item.Value)
            .Take(10))
        {
            Console.WriteLine(
                $"Dezena {item.Key:D2}: {item.Value} concursos");
        }

        Console.WriteLine();

        Console.WriteLine("Maiores dezenas mais frequentes:");

        foreach (var item in maiores
            .OrderByDescending(item => item.Value)
            .Take(10))
        {
            Console.WriteLine(
                $"Dezena {item.Key:D2}: {item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirDistribuicaoFaixas(
        Dictionary<string, Dictionary<int, int>> distribuicaoFaixas)
    {
        ExibirTitulo("DISTRIBUIÇÃO POR FAIXAS");

        foreach (var faixa in distribuicaoFaixas
            .OrderBy(item => item.Key))
        {
            Console.WriteLine($"Faixa {faixa.Key}:");

            foreach (var quantidade in faixa.Value
                .OrderBy(item => item.Key))
            {
                Console.WriteLine(
                    $"  {quantidade.Key} dezenas: {quantidade.Value} concursos");
            }

            Console.WriteLine();
        }
    }

    public void ExibirBaixasAltas(
        Dictionary<string, int> baixasAltas)
    {
        ExibirTitulo("DEZENAS BAIXAS E ALTAS");

        foreach (var item in baixasAltas
            .OrderByDescending(item => item.Value))
        {
            Console.WriteLine(
                $"{item.Key}: {item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirAtrasos(
        Dictionary<int, int> atrasos)
    {
        ExibirTitulo("ATRASO ATUAL");

        foreach (var item in atrasos
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Key))
        {
            Console.WriteLine(
                $"Dezena {item.Key:D2}: {item.Value} concursos de atraso");
        }

        Console.WriteLine();
    }

    public void ExibirFrequenciaRecente(
        Dictionary<int, int> frequenciaRecente,
        int quantidadeConcursos)
    {
        ExibirTitulo(
            $"FREQUÊNCIA NOS ÚLTIMOS {quantidadeConcursos} CONCURSOS");

        foreach (var item in frequenciaRecente
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Key))
        {
            Console.WriteLine(
                $"Dezena {item.Key:D2}: {item.Value} vezes");
        }

        Console.WriteLine();
    }

    public void ExibirMaiorAtraso(
        Dictionary<int, int> maioresAtrasos)
    {
        ExibirTitulo("MAIOR ATRASO HISTÓRICO");

        foreach (var item in maioresAtrasos
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Key))
        {
            Console.WriteLine(
                $"Dezena {item.Key:D2}: maior atraso de {item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirRepeticoes(
        Dictionary<int, int> repeticoes)
    {
        ExibirTitulo("REPETIÇÃO ENTRE CONCURSOS");

        foreach (var item in repeticoes
            .OrderBy(item => item.Key))
        {
            Console.WriteLine(
                $"{item.Key} dezenas repetidas: {item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirCoocorrencias(
        Dictionary<(int Dezena1, int Dezena2), int> coocorrencias)
    {
        ExibirTitulo("COOCORRÊNCIA DE DEZENAS");

        Console.WriteLine(
            "10 pares de dezenas que mais saíram juntos:");

        foreach (var item in coocorrencias
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Key.Dezena1)
            .ThenBy(item => item.Key.Dezena2)
            .Take(10))
        {
            Console.WriteLine(
                $"Dezenas {item.Key.Dezena1:D2} e {item.Key.Dezena2:D2}: " +
                $"{item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirMaioresSequencias(
        Dictionary<int, int> sequencias)
    {
        var maiorSequencia =
            sequencias.Values.Max();

        var concursosMaiorSequencia =
            sequencias
                .Where(item => item.Value == maiorSequencia)
                .Select(item => item.Key)
                .OrderBy(numero => numero)
                .ToList();

        ExibirTitulo("MAIORES SEQUÊNCIAS CONSECUTIVAS");

        Console.WriteLine(
            $"Maior sequência encontrada: {maiorSequencia} dezenas");

        Console.WriteLine(
            $"Quantidade de concursos: {concursosMaiorSequencia.Count}");

        Console.WriteLine(
            $"Concursos: {string.Join(", ", concursosMaiorSequencia)}");

        Console.WriteLine();
    }

    public void ExibirParesConsecutivos(
        Dictionary<int, int> paresConsecutivos)
    {
        ExibirTitulo("PARES CONSECUTIVOS");

        foreach (var item in paresConsecutivos
            .OrderBy(item => item.Key))
        {
            Console.WriteLine(
                $"{item.Key} pares consecutivos: " +
                $"{item.Value} concursos");
        }

        Console.WriteLine();
    }

    public void ExibirIntervalos(
        Dictionary<int, int> intervalos)
    {
        ExibirTitulo("INTERVALOS ENTRE DEZENAS");

        foreach (var item in intervalos
            .OrderBy(item => item.Key))
        {
            Console.WriteLine(
                $"Intervalo {item.Key}: {item.Value} ocorrências");
        }

        Console.WriteLine();
    }

    public void ExibirDistribuicaoSequencias(
        Dictionary<int, int> distribuicaoSequencias)
    {
        ExibirTitulo("DISTRIBUIÇÃO DAS MAIORES SEQUÊNCIAS");

        foreach (var item in distribuicaoSequencias
            .OrderBy(item => item.Key))
        {
            Console.WriteLine(
                $"{item.Key} dezenas consecutivas: " +
                $"{item.Value} concursos");
        }

        Console.WriteLine();
    }

    private static void ExibirTitulo(string titulo)
    {
        Console.WriteLine("==============================================");
        Console.WriteLine($"       {titulo}");
        Console.WriteLine("==============================================");
        Console.WriteLine();
    }
}