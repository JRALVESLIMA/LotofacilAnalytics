using LotofacilAnalytics.App.Presentation;
using LotofacilAnalytics.App.Services;

var raizProjeto = EncontrarRaizProjeto();

var diretorioRaw = Path.Combine(
    raizProjeto,
    "data",
    "raw");

var reporter = new ConsoleReporter();

reporter.ExibirCabecalho();

Console.WriteLine("Carregando histórico da Lotofácil...");
Console.WriteLine();

var aplicacaoService =
    new AplicacaoService(diretorioRaw);

var concursos =
    await aplicacaoService.CarregarHistoricoAsync();

reporter.ExibirResumoHistorico(concursos);

var relatorio =
    aplicacaoService.ExecutarAnalises(concursos);

reporter.ExibirRelatorio(relatorio);

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