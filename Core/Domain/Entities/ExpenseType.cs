namespace Domain.Entities;
public class ExpenseType
{
    public int Id { get; set; }
    public string ExpenseTypeName { get; set; } 
    public string ExpenseTypeDescription { get; set; }
    public string Status { get; set; } 
    public string UserAdded { get; set; } 
    public DateTime DateAdded { get; set; }

    public ICollection<Expenses> Expenses { get; set; } 
}