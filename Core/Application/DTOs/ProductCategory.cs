namespace Application.DTOs;

public class AddProductCategoryDTO
{
    public int Id { get; set; }
    public string ProductCategoryName { get; set; } = string.Empty;
    public string ProductCategoryDescription { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}

public class GetProductCategoryDTO
{
    public int Id { get; set; }
    public string ProductCategoryName { get; set; } = string.Empty;
    public string ProductCategoryDescription { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}
