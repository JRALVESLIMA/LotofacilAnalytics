namespace LotofacilAnalytics.Domain.Entities;

public class Concurso
{
    public int Numero { get; }

    public DateTime DataSorteio { get; }

    public IReadOnlyCollection<int> Dezenas { get; }

    public Concurso(
        int numero,
        DateTime dataSorteio,
        IEnumerable<int> dezenas)
    {
        if (numero <= 0)
        {
            throw new ArgumentException(
                "O número do concurso deve ser maior que zero.",
                nameof(numero));
        }

        if (dataSorteio == default)
        {
            throw new ArgumentException(
                "A data do sorteio deve ser válida.",
                nameof(dataSorteio));
        }

        var listaDezenas = dezenas?.ToList()
            ?? throw new ArgumentNullException(nameof(dezenas));

        if (listaDezenas.Count != 15)
        {
            throw new ArgumentException(
                "O concurso deve possuir exatamente 15 dezenas.",
                nameof(dezenas));
        }

        if (listaDezenas.Any(dezena => dezena < 1 || dezena > 25))
        {
            throw new ArgumentException(
                "As dezenas devem estar entre 1 e 25.",
                nameof(dezenas));
        }

        if (listaDezenas.Distinct().Count() != 15)
        {
            throw new ArgumentException(
                "O concurso não pode possuir dezenas repetidas.",
                nameof(dezenas));
        }

        Dezenas = listaDezenas
            .Order()
            .ToArray();

        Numero = numero;
        DataSorteio = dataSorteio;
    }
}