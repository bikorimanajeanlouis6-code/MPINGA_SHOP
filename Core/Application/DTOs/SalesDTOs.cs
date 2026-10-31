namespace Application.DTOs;

public class AddSalesDTO
{
    public int CustomerId { get; set; }
    public string TotalConsumed { get; set; } = "0";
    public string Balance { get; set; } = "0";
    public string TotalPaid { get; set; } = "0";
    public DateTime SaleDate { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetSalesDTO
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string TotalConsumed { get; set; } = string.Empty;
    public string Balance { get; set; } = string.Empty;
    public string TotalPaid { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
