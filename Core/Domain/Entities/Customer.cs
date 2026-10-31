namespace Domain.Entities;
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Email { get; set; } 
    public string PhoneNumber { get; set; }
    public string AmountConsumed { get; set; } 
    public string AmountPaid { get; set; }
    public string Balance { get; set; } 
    public string Address { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; }
    public string Status { get; set; } 

    public ICollection<Sales> Sales { get; set; } 
}
