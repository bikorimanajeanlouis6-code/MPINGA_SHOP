namespace Domain.Entities;
public class PaymentMethod
{
    public int Id { get; set; }
    public string PaymentMethodName { get; set; } 
    public string Status { get; set; } 
    public string UserAdded { get; set; } 
    public DateTime DataAdded { get; set; }

    public ICollection<Payment> Payments { get; set; } 
}