namespace Domain.Entities;

public class AccountDeposit
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;

    public Account Account { get; set; } = null!;
}
