namespace Domain.Entities;
public class Replenishment
{
    public int Id { get; set; }
    public string ReplenishmentName { get; set; }
    public string ReplenishmentAmount { get; set; }
    public string ReplenishmentDate { get; set; } 
    public string QuantityReplenished { get; set; }
    public string SellingPrice { get; set; }
    public string PurchasePrice { get; set; }
    public string totalpurchaseprice { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string SupplierPhone { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; }
    public string Status { get; set; }
    public int ProductId { get; set; }

/// relationship

    public Product Product { get; set;}
}