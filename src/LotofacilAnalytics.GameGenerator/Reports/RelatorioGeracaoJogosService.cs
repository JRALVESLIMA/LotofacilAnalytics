
using LotofacilAnalytics.GameGenerator.Models;

namespace LotofacilAnalytics.GameGenerator.Reports;

public sealed class RelatorioGeracaoJogosService
{
    public RelatorioGeracaoJogos Criar(
        string nomeEstrategia,
        GeracaoJogosRequest request,
        GeracaoJogosResultado resultado)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(resultado);

        if (resultado.QuantidadeSolicitada != request.QuantidadeJogos)
        {
            throw new ArgumentException(
                "A quantidade solicitada no resultado deve " +
                "corresponder à quantidade informada no request.",
                nameof(resultado));
        }

        if (resultado.QuantidadeGerada != request.QuantidadeJogos)
        {
            throw new ArgumentException(
                "O resultado deve conter todos os jogos solicitados.",
                nameof(resultado));
        }

        int quantidadeJogosOriginais =
            request.DiversificarJogos
                ? 1
                : resultado.QuantidadeGerada;

        return new RelatorioGeracaoJogos(
            nomeEstrategia,
            request.QuantidadeJogos,
            resultado.QuantidadeGerada,
            request.DiversificarJogos,
            quantidadeJogosOriginais);
    }
}