namespace Domain.Entities;
public class ProductCategory
{
    public int Id { get; set; }
    public string ProductCategoryName { get; set; }
    public string ProductCategoryDescription { get; set; }
    public string Status { get; set; }
    public string UserAdded { get; set; }
    public DateTime DataAdded { get; set; }

    public ICollection<Product> Products { get; set; }
}