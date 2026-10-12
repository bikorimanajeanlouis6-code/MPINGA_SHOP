
namespace Application.DTOs;

public class AddProductDTO
{
    public int Id { get; set; }

    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string ProductPrice { get; set; } = string.Empty;
    public string ProductQuantity { get; set; } = string.Empty;
    public int ProductCategoryId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}

public class GetProductDTO
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string ProductPrice { get; set; } = string.Empty;
    public string ProductQuantity { get; set; } = string.Empty;
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}
