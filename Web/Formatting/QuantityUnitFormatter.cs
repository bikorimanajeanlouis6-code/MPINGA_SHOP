namespace Web.Formatting;

public static class QuantityUnitFormatter
{
    public static string Format(string? unit)
    {
        if (string.IsNullOrWhiteSpace(unit))
            return "unit not set";

        var normalized = unit.Trim();
        if (normalized.Equals("piece", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("pieces", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("pc", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("pcs", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("pieces (pcs)", StringComparison.OrdinalIgnoreCase))
            return "pcs";

        if (normalized.Equals("kg", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("kgs", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("kilogram", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("kilograms", StringComparison.OrdinalIgnoreCase))
            return "kg";

        return normalized;
    }
}
