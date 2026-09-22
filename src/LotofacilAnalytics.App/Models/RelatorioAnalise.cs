namespace LotofacilAnalytics.App.Models;

public class RelatorioAnalise
{
    public Dictionary<int, int> Frequencias { get; init; } = [];

    public Dictionary<string, int> ParesImpares { get; init; } = [];

    public Dictionary<int, int> Somas { get; init; } = [];

    public Dictionary<int, int> Menores { get; init; } = [];

    public Dictionary<int, int> Maiores { get; init; } = [];

    public Dictionary<string, int> DistribuicaoSomas { get; init; } = [];

    public Dictionary<string, Dictionary<int, int>> DistribuicaoFaixas { get; init; } = [];

    public Dictionary<string, int> BaixasAltas { get; init; } = [];

    public Dictionary<int, int> Atrasos { get; init; } = [];

    public Dictionary<int, int> FrequenciaRecente { get; init; } = [];

    public int ConcursosRecentes { get; init; }

    public Dictionary<int, int> MaioresAtrasos { get; init; } = [];

    public Dictionary<int, int> Repeticoes { get; init; } = [];

    public Dictionary<(int Dezena1, int Dezena2), int> Coocorrencias { get; init; } = [];

    public Dictionary<int, int> Sequencias { get; init; } = [];

    public Dictionary<int, int> ParesConsecutivos { get; init; } = [];

    public Dictionary<int, int> Intervalos { get; init; } = [];

    public Dictionary<int, int> DistribuicaoSequencias { get; init; } = [];
}