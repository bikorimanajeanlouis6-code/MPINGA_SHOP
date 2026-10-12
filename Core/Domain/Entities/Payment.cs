namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string PaymentName { get; set; } 
    public string PaymentAmount { get; set; } 
    public string PaymentDate { get; set; } 
    public string PaymentDescription { get; set; } 
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } 
    public string Status { get; set; } 

    public int? SalesId { get; set; }
    public Sales? Sales { get; set; }

    public int PaymentMethodId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }

    public int? AccountId { get; set; }
    public Account? Account { get; set; }
}