namespace LotofacilAnalytics.GameGenerator.Validators;

public sealed class ValidadorJogo
{
    public void Validar(IReadOnlyCollection<int> dezenas)
    {
        ArgumentNullException.ThrowIfNull(dezenas);

        if (dezenas.Count != 15)
        {
            throw new ArgumentException(
                "O jogo deve conter exatamente 15 dezenas.",
                nameof(dezenas));
        }

        if (dezenas.Any(dezena => dezena < 1 || dezena > 25))
        {
            throw new ArgumentException(
                "As dezenas devem estar entre 1 e 25.",
                nameof(dezenas));
        }

        if (dezenas.Distinct().Count() != 15)
        {
            throw new ArgumentException(
                "O jogo não pode conter dezenas repetidas.",
                nameof(dezenas));
        }
    }
}