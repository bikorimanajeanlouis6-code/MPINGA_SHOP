namespace Application.DTOs;

public class AddReplenishmentDTO
{
    public int Id { get; set; }

    public string ReplenishmentName { get; set; } = string.Empty;
    public string ReplenishmentAmount { get; set; } = string.Empty;
    public DateTime ReplenishmentDate { get; set; }
    public int QuantityReplenished { get; set; }
    public string SellingPrice { get; set; } = string.Empty;
    public string PurchasePrice { get; set; } = string.Empty;
    public string TotalPurchasePrice { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public string SupplierPhone { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetReplenishmentDTO
{
    public int Id { get; set; }
    public string ReplenishmentName { get; set; } = string.Empty;
    public string ReplenishmentAmount { get; set; } = string.Empty;
    public DateTime ReplenishmentDate { get; set; }
    public int QuantityReplenished { get; set; }
    public string SellingPrice { get; set; } = string.Empty;
    public string PurchasePrice { get; set; } = string.Empty;
    public string TotalPurchasePrice { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public string SupplierPhone { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}