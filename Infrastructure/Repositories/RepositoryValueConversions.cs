using System.Globalization;

namespace Infrastructure.Repositories;

internal static class RepositoryValueConversions
{
    public static string ToStorageString(decimal value) =>
        value.ToString(CultureInfo.InvariantCulture);

    public static string ToStorageString(string value) =>
        value ?? string.Empty;

    public static string ToStorageString(int value) =>
        value.ToString(CultureInfo.InvariantCulture);

    public static string ToStorageString(DateTime value) =>
        value.ToString("O", CultureInfo.InvariantCulture);

    public static decimal ToDecimal(string value) =>
        decimal.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);

    public static int ToInt32(string value) =>
        int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);

    public static DateTime ToDateTime(string value) =>
        DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
}
