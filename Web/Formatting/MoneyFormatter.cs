using System.Globalization;

namespace Web.Formatting;

public static class MoneyFormatter
{
    public static string Format(decimal amount) =>
        amount.ToString("N2", CultureInfo.CurrentCulture);

    public static string Format(string? amount)
    {
        if (string.IsNullOrWhiteSpace(amount))
            return string.Empty;

        if (decimal.TryParse(amount, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            || decimal.TryParse(amount, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
        {
            return Format(value);
        }

        return amount;
    }
}
