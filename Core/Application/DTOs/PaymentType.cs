namespace Application.DTOs;

public class AddPaymentMethordDTO
{
    public int Id { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}

public class GetPaymentMethordDTO
{
    public int Id { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}