namespace Application.DTOs;

public class AddStockDTO
{
    public int Id { get; set; }
    public string StockName { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
    public int QuantitySold { get; set; }
    public int QuantityAvailable { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal LastPrice { get; set; }
    public DateTime LastlyModified { get; set; }
    public int ProductId { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetStockDTO
{
    public int Id { get; set; }
    public string StockName { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
    public int QuantitySold { get; set; }
    public int QuantityAvailable { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal LastPrice { get; set; }
    public DateTime LastlyModified { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}