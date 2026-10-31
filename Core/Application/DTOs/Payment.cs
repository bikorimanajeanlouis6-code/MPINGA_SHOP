namespace Application.DTOs;

public class AddPaymentDTO
{
    public int Id { get; set; }
    public string PaymentName { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentDescription { get; set; } = string.Empty;
    public int PaymentMethodId { get; set; }
    public int? SalesId { get; set; }
    public int? AccountId { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetPaymentDTO
{
    public int Id { get; set; }
    public string PaymentName { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentDescription { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}