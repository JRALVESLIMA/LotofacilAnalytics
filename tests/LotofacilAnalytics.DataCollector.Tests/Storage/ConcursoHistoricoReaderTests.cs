using System.Text.Json;
using LotofacilAnalytics.DataCollector.Mappers;
using LotofacilAnalytics.DataCollector.Models;
using LotofacilAnalytics.DataCollector.Storage;
using LotofacilAnalytics.DataCollector.Validation;

namespace LotofacilAnalytics.DataCollector.Tests.Storage;

public class ConcursoHistoricoReaderTests
{
    [Fact]
    public async Task DeveLerEMapearConcursosDoHistorico()
    {
        var diretorio = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString());

        Directory.CreateDirectory(diretorio);

        try
        {
            var concurso1 = new ConcursoCaixa
            {
                Numero = 1,
                DataApuracao = "29/09/2003",
                ListaDezenas =
                [
                    "02", "03", "05", "06", "09",
                    "10", "11", "13", "14", "16",
                    "18", "20", "23", "24", "25"
                ]
            };

            var concurso2 = new ConcursoCaixa
            {
                Numero = 2,
                DataApuracao = "06/10/2003",
                ListaDezenas =
                [
                    "01", "02", "03", "04", "06",
                    "08", "09", "10", "12", "14",
                    "16", "18", "20", "22", "24"
                ]
            };

            await SalvarConcursoAsync(
                diretorio,
                concurso1);

            await SalvarConcursoAsync(
                diretorio,
                concurso2);

            var validator = new ConcursoValidator();
            var mapper = new ConcursoMapper();

            var reader = new ConcursoHistoricoReader(
                diretorio,
                validator,
                mapper);

            var resultado = await reader.LerTodosAsync();

            Assert.Equal(2, resultado.Count);

            Assert.Equal(1, resultado[0].Numero);
            Assert.Equal(
                new DateTime(2003, 9, 29),
                resultado[0].DataSorteio);

            Assert.Equal(2, resultado[1].Numero);
            Assert.Equal(
                new DateTime(2003, 10, 6),
                resultado[1].DataSorteio);

            Assert.Equal(15, resultado[0].Dezenas.Count);
            Assert.Equal(15, resultado[1].Dezenas.Count);
        }
        finally
        {
            Directory.Delete(
                diretorio,
                recursive: true);
        }
    }

    [Fact]
    public async Task DeveRejeitarConcursoInvalido()
    {
        var diretorio = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString());

        Directory.CreateDirectory(diretorio);

        try
        {
            var concursoInvalido = new ConcursoCaixa
            {
                Numero = 1,
                DataApuracao = "29/09/2003",
                ListaDezenas =
                [
                    "01", "02", "03"
                ]
            };

            await SalvarConcursoAsync(
                diretorio,
                concursoInvalido);

            var validator = new ConcursoValidator();
            var mapper = new ConcursoMapper();

            var reader = new ConcursoHistoricoReader(
                diretorio,
                validator,
                mapper);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => reader.LerTodosAsync());
        }
        finally
        {
            Directory.Delete(
                diretorio,
                recursive: true);
        }
    }

    private static async Task SalvarConcursoAsync(
        string diretorio,
        ConcursoCaixa concurso)
    {
        var nomeArquivo =
            $"concurso_{concurso.Numero:D4}.json";

        var caminho =
            Path.Combine(diretorio, nomeArquivo);

        var json = JsonSerializer.Serialize(
            concurso,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(caminho, json);
    }
}