namespace LotofacilAnalytics.DataCollector.Models;

public class ConcursoCaixa
{
    public int Numero {  get; set; }
    public string DataApuracao { get; set; } = string.Empty;
    public List<string> ListaDezenas { get; set; } = [];
}