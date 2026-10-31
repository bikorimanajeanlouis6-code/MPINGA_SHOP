namespace Application.DTOs;

public class AddSaleItemsDTO
{
    public string SaleItemName { get; set; } = string.Empty;
    public string SaleItemPrice { get; set; } = string.Empty;
    public string SaleItemQuantity { get; set; } = string.Empty;
    public string TotalSaleItemPrice { get; set; } = string.Empty;
    public string SaleItemCategory { get; set; } = string.Empty;
    public int SalesId { get; set; }
    public int ProductId { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetSaleItemsDTO
{
    public int Id { get; set; }
    public string SaleItemName { get; set; } = string.Empty;
    public string SaleItemPrice { get; set; } = string.Empty;
    public string SaleItemQuantity { get; set; } = string.Empty;
    public string TotalSaleItemPrice { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string SaleItemCategory { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int SalesId { get; set; }
    public int ProductId { get; set; }
}