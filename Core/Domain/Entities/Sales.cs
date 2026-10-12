namespace Domain.Entities;
public class Sales
{
    public int Id { get; set; }
    public string TotalConsumed { get; set; } = "0";
    public string Balance { get; set; } = "0";
    public string TotalPaid { get; set; } = "0";
    public DateTime SaleDate { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }
    public ICollection<SaleItems> SaleItems { get; set; } = new List<SaleItems>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}