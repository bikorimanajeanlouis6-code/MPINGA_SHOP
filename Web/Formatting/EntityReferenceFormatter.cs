using System.Globalization;

namespace Web.Formatting;

public enum EntityReferenceType
{
    Account,
    AccountDeposit,
    Customer,
    Expense,
    ExpenseType,
    Payment,
    PaymentMethod,
    Product,
    ProductCategory,
    Replenishment,
    Sale,
    SaleItem,
    Stock,
    User
}

public static class EntityReferenceFormatter
{
    public static string Format(EntityReferenceType entityType, int id)
    {
        var prefix = entityType switch
        {
            EntityReferenceType.Account => "AC",
            EntityReferenceType.AccountDeposit => "DP",
            EntityReferenceType.Customer => "CU",
            EntityReferenceType.Expense => "EX",
            EntityReferenceType.ExpenseType => "ET",
            EntityReferenceType.Payment => "PA",
            EntityReferenceType.PaymentMethod => "PM",
            EntityReferenceType.Product => "PR",
            EntityReferenceType.ProductCategory => "PC",
            EntityReferenceType.Replenishment => "RP",
            EntityReferenceType.Sale => "SA",
            EntityReferenceType.SaleItem => "SI",
            EntityReferenceType.Stock => "ST",
            EntityReferenceType.User => "US",
            _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
        };

        return $"{prefix}{DateTime.Now.Year}{id.ToString("D3", CultureInfo.InvariantCulture)}";
    }
}
