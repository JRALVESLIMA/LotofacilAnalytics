using System.Text.Json;
using LotofacilAnalytics.DataCollector.Models;

namespace LotofacilAnalytics.DataCollector.Validation;

public class BaseHistoricaAuditor
{
    private readonly string _diretorio;
    private readonly ConcursoValidator _validator;

    public BaseHistoricaAuditor(
        string diretorio,
        ConcursoValidator validator)
    {
        _diretorio = diretorio;
        _validator = validator;
    }

    public async Task AuditarAsync()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("AUDITORIA DA BASE HISTÓRICA");
        Console.WriteLine("========================================");
        Console.WriteLine();

        var arquivos = Directory.GetFiles(
            _diretorio,
            "concurso_*.json");

        Console.WriteLine($"Total de arquivos: {arquivos.Length}");

        if (arquivos.Length == 0)
        {
            Console.WriteLine();
            Console.WriteLine("ERRO: Nenhum arquivo encontrado.");
            return;
        }

        var concursos = new List<ConcursoCaixa>();

        var jsonsInvalidos = 0;
        var numerosIncorretos = 0;
        var concursosInvalidos = 0;

        foreach (var arquivo in arquivos.OrderBy(x => x))
        {
            try
            {
                var json = await File.ReadAllTextAsync(arquivo);

                var concurso =
                    JsonSerializer.Deserialize<ConcursoCaixa>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (concurso is null)
                {
                    jsonsInvalidos++;
                    continue;
                }

                var nomeArquivo =
                    Path.GetFileNameWithoutExtension(arquivo);

                var numeroTexto =
                    nomeArquivo.Replace("concurso_", "");

                if (!int.TryParse(
                        numeroTexto,
                        out var numeroArquivo))
                {
                    numerosIncorretos++;
                    continue;
                }

                if (concurso.Numero != numeroArquivo)
                {
                    numerosIncorretos++;
                    continue;
                }

                if (!_validator.Validar(concurso))
                {
                    concursosInvalidos++;
                    continue;
                }

                concursos.Add(concurso);
            }
            catch (JsonException)
            {
                jsonsInvalidos++;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Erro ao ler o arquivo: {arquivo}");

                Console.WriteLine(
                    $"Mensagem: {ex.Message}");
            }
        }

        if (concursos.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine(
                "ERRO: Nenhum concurso válido foi encontrado.");

            return;
        }

        var primeiroConcurso =
            concursos.Min(x => x.Numero);

        var ultimoConcurso =
            concursos.Max(x => x.Numero);

        var numerosExistentes =
            concursos
                .Select(x => x.Numero)
                .ToHashSet();

        var concursosFaltantes = new List<int>();

        for (
            var numero = primeiroConcurso;
            numero <= ultimoConcurso;
            numero++)
        {
            if (!numerosExistentes.Contains(numero))
            {
                concursosFaltantes.Add(numero);
            }
        }

        Console.WriteLine();
        Console.WriteLine(
            $"Primeiro concurso: {primeiroConcurso}");

        Console.WriteLine(
            $"Último concurso: {ultimoConcurso}");

        Console.WriteLine();
        Console.WriteLine(
            $"Concursos válidos: {concursos.Count}");

        Console.WriteLine(
            $"Concursos faltantes: {concursosFaltantes.Count}");

        Console.WriteLine(
            $"JSONs inválidos: {jsonsInvalidos}");

        Console.WriteLine(
            $"Concursos com número incorreto: {numerosIncorretos}");

        Console.WriteLine(
            $"Concursos inválidos: {concursosInvalidos}");

        if (concursosFaltantes.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Concursos faltantes:");

            foreach (var numero in concursosFaltantes)
            {
                Console.WriteLine($"    - {numero}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("========================================");

        if (
            concursosFaltantes.Count == 0 &&
            jsonsInvalidos == 0 &&
            numerosIncorretos == 0 &&
            concursosInvalidos == 0)
        {
            Console.WriteLine(
                "BASE HISTÓRICA ÍNTEGRA ✓");
        }
        else
        {
            Console.WriteLine(
                "BASE HISTÓRICA POSSUI PROBLEMAS ⚠");
        }

        Console.WriteLine("========================================");
    }
}