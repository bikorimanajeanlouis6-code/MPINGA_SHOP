namespace Domain.Entities;
public class Stock
{
    public int Id { get; set; }
    public string StockName { get; set; } 
    public string QuantityInStock { get; set; } 
    public string QuantitySold { get; set; }
    public string QuantityAvailable { get; set; } 
    public string Unit { get; set; } 
    public string SellingPrice { get; set; } 
    public string LastPrice { get; set; }
    public string LastlyModified { get; set; } 
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } 
    public string Status { get; set; } 

    /// relationship
    public int ProductId { get; set; }
    public Product Product { get; set; }
}