using LotofacilAnalytics.DataCollector.Clients;
using LotofacilAnalytics.DataCollector.Storage;
using LotofacilAnalytics.DataCollector.Validation;
using LotofacilAnalytics.DataCollector.Mappers;

namespace LotofacilAnalytics.DataCollector.Services;

public class ConcursoUpdateService
{
    private readonly CaixaApiClient _caixaApiClient;
    private readonly ConcursoJsonStorage _storage;
    private readonly ConcursoValidator _validator;
    private readonly ConcursoMapper _mapper;

    public ConcursoUpdateService(
        CaixaApiClient caixaApiClient,
        ConcursoJsonStorage storage,
        ConcursoValidator validator,
        ConcursoMapper mapper)
    {
        _caixaApiClient = caixaApiClient;
        _storage = storage;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task AtualizarAsync(int? limite = null)
    {
        var ultimoConcursoLocal =
            await _storage.ObterUltimoConcursoSalvoAsync();

        var ultimoConcursoCaixa =
            await _caixaApiClient.ObterUltimoConcursoAsync();

        if (ultimoConcursoCaixa is null)
        {
            Console.WriteLine(
                "Não foi possível obter o último concurso da CAIXA.");

            return;
        }

        var numeroLocal =
            ultimoConcursoLocal ?? 0;

        var numeroCaixa =
            ultimoConcursoCaixa.Numero;

        Console.WriteLine(
            $"Último concurso local: {numeroLocal}");

        Console.WriteLine(
            $"Último concurso CAIXA: {numeroCaixa}");

        if (numeroLocal >= numeroCaixa)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Os dados locais já estão atualizados.");

            return;
        }

        var quantidadeFaltante =
            numeroCaixa - numeroLocal;

        var quantidadeProcessar =
            limite.HasValue
                ? Math.Min(limite.Value, quantidadeFaltante)
                : quantidadeFaltante;

        var concursoInicial =
            numeroLocal + 1;

        var concursoFinal =
            numeroLocal + quantidadeProcessar;

        Console.WriteLine();
        Console.WriteLine(
            $"Concursos faltantes: {quantidadeFaltante}");

        Console.WriteLine(
            $"Concursos a processar agora: {quantidadeProcessar}");

        Console.WriteLine(
            $"Intervalo: {concursoInicial} até {concursoFinal}");

        Console.WriteLine();

        var processados = 0;

        for (
            var numeroConcurso = concursoInicial;
            numeroConcurso <= concursoFinal;
            numeroConcurso++)
        {
            processados++;

            Console.WriteLine(
                $"[{processados}/{quantidadeProcessar}] " +
                $"Buscando concurso {numeroConcurso}...");

            try
            {
                var concurso =
                    await _caixaApiClient.ObterConcursoAsync(
                        numeroConcurso);

                if (concurso is null)
                {
                    Console.WriteLine(
                        $"ERRO: A CAIXA não retornou o concurso {numeroConcurso}.");

                    return;
                }

                if (concurso.Numero != numeroConcurso)
                {
                    Console.WriteLine(
                        $"ERRO: Foi solicitado o concurso {numeroConcurso}, " +
                        $"mas a API retornou o concurso {concurso.Numero}.");

                    return;
                }

                if (!_validator.Validar(concurso))
                {
                    Console.WriteLine(
                        $"ERRO: O concurso {numeroConcurso} " +
                        $"falhou na validação.");

                    return;
                }

                var concursoDominio = _mapper.Mapear(concurso);

                await _storage.SalvarAsync(concurso);

                Console.WriteLine(
                    $"    ✓ Concurso {numeroConcurso} salvo com sucesso.");

                Console.WriteLine(
                    $"    Data: {concurso.DataApuracao}");

                Console.WriteLine(
                    $"    Dezenas: {string.Join(" ", concurso.ListaDezenas)}");

                Console.WriteLine();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"ERRO HTTP ao buscar o concurso {numeroConcurso}.");

                Console.WriteLine(
                    $"Mensagem: {ex.Message}");

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"ERRO inesperado no concurso {numeroConcurso}.");

                Console.WriteLine(
                    $"Mensagem: {ex.Message}");

                return;
            }

            // Pequena pausa para não bombardear a API.
            if (numeroConcurso < concursoFinal)
            {
                await Task.Delay(300);
            }
        }

        Console.WriteLine(
            "========================================");

        Console.WriteLine(
            "Atualização concluída!");

        Console.WriteLine(
            $"Concursos processados: {processados}");

        Console.WriteLine(
            $"Intervalo processado: {concursoInicial} até {concursoFinal}");

        Console.WriteLine(
            "========================================");
    }
}