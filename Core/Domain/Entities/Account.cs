
namespace Domain.Entities;

public class Account
{
    public int Id { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; } 
    public string BankName { get; set; } 
    public string Status { get; set; } 
    public string UserAdded { get; set; } 
    public DateTime DateAdded { get; set; }

    public ICollection<Payment> Payments { get; set; }
}