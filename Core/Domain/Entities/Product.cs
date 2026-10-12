
namespace Domain.Entities;
public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; } = string.Empty;
    public string ProductPrice { get; set; }
    public string ProductQuantity { get; set; }
    public int ProductCategoryId { get; set; }
    public ProductCategory ProductCategory { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; }
    public string Status { get; set; }

    public ICollection<Stock> Stocks { get; set; }
    public ICollection<SaleItems> SaleItems { get; set; }
    public ICollection<Replenishment> Replenishments { get; set; }
}