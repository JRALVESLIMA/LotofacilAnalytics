using LotofacilAnalytics.Analysis.Services;
using LotofacilAnalytics.DataCollector.Mappers;
using LotofacilAnalytics.DataCollector.Storage;
using LotofacilAnalytics.DataCollector.Validation;

var raizProjeto = EncontrarRaizProjeto();

var diretorioRaw = Path.Combine(
raizProjeto,
"data",
"raw");

ExibirCabecalho();

Console.WriteLine("Carregando histórico da Lotofácil...");
Console.WriteLine();

var validator = new ConcursoValidator();
var mapper = new ConcursoMapper();

var reader = new ConcursoHistoricoReader(
diretorioRaw,
validator,
mapper);

var concursos = await reader.LerTodosAsync();

Console.WriteLine(
$"Concursos carregados: {concursos.Count}");

Console.WriteLine(
$"Primeiro concurso: {concursos.Min(c => c.Numero)}");

Console.WriteLine(
$"Último concurso: {concursos.Max(c => c.Numero)}");

Console.WriteLine();

var service = new AnaliseEstatisticaService();

var frequencias =
service.CalcularFrequencia(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("       FREQUÊNCIA DAS DEZENAS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in frequencias
.OrderBy(item => item.Key))
{
Console.WriteLine(
$"Dezena {item.Key:D2}: {item.Value} vezes");
}

Console.WriteLine();

var paresImpares =
service.CalcularParesImpares(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("          PARES E ÍMPARES");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in paresImpares
.OrderByDescending(item => item.Value))
{
Console.WriteLine(
$"{item.Key}: {item.Value} concursos");
}

Console.WriteLine();

var somas =
service.CalcularSomas(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("           SOMA DAS DEZENAS");
Console.WriteLine("==============================================");
Console.WriteLine();

Console.WriteLine(
$"Menor soma encontrada: {somas.Keys.Min()}");

Console.WriteLine(
$"Maior soma encontrada: {somas.Keys.Max()}");

Console.WriteLine();

var menores =
service.CalcularMenoresDezenas(concursos);

var maiores =
service.CalcularMaioresDezenas(concursos);

var distribuicaoSomas =
    service.CalcularDistribuicaoSomas(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("          DISTRIBUIÇÃO DAS SOMAS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in distribuicaoSomas
    .OrderBy(item => item.Key))
{
    Console.WriteLine(
        $"Soma {item.Key}: {item.Value} concursos");
}

Console.WriteLine();

Console.WriteLine("==============================================");
Console.WriteLine("        MENOR E MAIOR DEZENA");
Console.WriteLine("==============================================");
Console.WriteLine();

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

var distribuicaoFaixas =
service.CalcularDistribuicaoFaixasHistorica(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("       DISTRIBUIÇÃO POR FAIXAS");
Console.WriteLine("==============================================");
Console.WriteLine();

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

var baixasAltas =
    service.CalcularBaixasAltas(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("          DEZENAS BAIXAS E ALTAS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in baixasAltas
    .OrderByDescending(item => item.Value))
{
    Console.WriteLine(
        $"{item.Key}: {item.Value} concursos");
}

Console.WriteLine();

var atrasos =
service.CalcularAtrasos(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("            ATRASO ATUAL");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in atrasos
.OrderByDescending(item => item.Value)
.ThenBy(item => item.Key))
{
Console.WriteLine(
$"Dezena {item.Key:D2}: {item.Value} concursos de atraso");
}

Console.WriteLine();

const int concursosRecentes = 20;

var frequenciaRecente =
service.CalcularFrequenciaRecente(
concursos,
concursosRecentes);

Console.WriteLine("==============================================");
Console.WriteLine(
$"   FREQUÊNCIA NOS ÚLTIMOS {concursosRecentes} CONCURSOS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in frequenciaRecente
.OrderByDescending(item => item.Value)
.ThenBy(item => item.Key))
{
Console.WriteLine(
$"Dezena {item.Key:D2}: {item.Value} vezes");
}

Console.WriteLine();

var maioresAtrasos =
service.CalcularMaiorAtraso(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("        MAIOR ATRASO HISTÓRICO");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in maioresAtrasos
.OrderByDescending(item => item.Value)
.ThenBy(item => item.Key))
{
Console.WriteLine(
$"Dezena {item.Key:D2}: maior atraso de {item.Value} concursos");
}

Console.WriteLine();

var repeticoes =
service.CalcularRepeticoes(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("       REPETIÇÃO ENTRE CONCURSOS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in repeticoes
.OrderBy(item => item.Key))
{
Console.WriteLine(
$"{item.Key} dezenas repetidas: {item.Value} concursos");
}

Console.WriteLine();

var sequencias =
service.CalcularMaioresSequencias(concursos);

var maiorSequencia =
sequencias.Values.Max();

var concursosMaiorSequencia =
sequencias
.Where(item => item.Value == maiorSequencia)
.Select(item => item.Key)
.OrderBy(numero => numero)
.ToList();

var coocorrencias =
    service.CalcularCoocorrencia(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("       COOCORRÊNCIA DE DEZENAS");
Console.WriteLine("==============================================");
Console.WriteLine();

Console.WriteLine("10 pares de dezenas que mais saíram juntos:");

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


Console.WriteLine("==============================================");
Console.WriteLine("       MAIORES SEQUÊNCIAS CONSECUTIVAS");
Console.WriteLine("==============================================");
Console.WriteLine();

Console.WriteLine(
$"Maior sequência encontrada: {maiorSequencia} dezenas");

Console.WriteLine(
$"Quantidade de concursos: {concursosMaiorSequencia.Count}");

Console.WriteLine(
$"Concursos: {string.Join(", ", concursosMaiorSequencia)}");

Console.WriteLine();

var paresConsecutivos =
    service.CalcularParesConsecutivos(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("          PARES CONSECUTIVOS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in paresConsecutivos
    .OrderBy(item => item.Key))
{
    Console.WriteLine(
        $"{item.Key} pares consecutivos: " +
        $"{item.Value} concursos");
}

Console.WriteLine();

var intervalos =
    service.CalcularIntervalos(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("       INTERVALOS ENTRE DEZENAS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in intervalos
    .OrderBy(item => item.Key))
{
    Console.WriteLine(
        $"Intervalo {item.Key}: {item.Value} ocorrências");
}

Console.WriteLine();

var distribuicaoSequencias =
    service.CalcularDistribuicaoSequencias(concursos);

Console.WriteLine("==============================================");
Console.WriteLine("     DISTRIBUIÇÃO DAS MAIORES SEQUÊNCIAS");
Console.WriteLine("==============================================");
Console.WriteLine();

foreach (var item in distribuicaoSequencias
    .OrderBy(item => item.Key))
{
    Console.WriteLine(
        $"{item.Key} dezenas consecutivas: " +
        $"{item.Value} concursos");
}

Console.WriteLine();

static void ExibirCabecalho()
{
Console.WriteLine("==============================================");
Console.WriteLine("           LOTOFÁCIL ANALYTICS");
Console.WriteLine("==============================================");
Console.WriteLine();
}

static string EncontrarRaizProjeto()
{
var diretorio = new DirectoryInfo(
AppContext.BaseDirectory);

while (diretorio is not null)
{
    var diretorioRaw = Path.Combine(
        diretorio.FullName,
        "data",
        "raw");

    if (Directory.Exists(diretorioRaw))
        return diretorio.FullName;

    diretorio = diretorio.Parent;
}

throw new DirectoryNotFoundException(
    "Não foi possível localizar a raiz do projeto.");

}
