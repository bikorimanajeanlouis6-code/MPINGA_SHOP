namespace Domain.Entities;
public class Expenses
{
    public int Id { get; set; }
    public string ExpenseName { get; set; }
    public string ExpenseAmount { get; set; } 
    public DateTime ExpenseDate { get; set; } 
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } 
    public string Status { get; set; } 
    public int ExpenseTypeId { get; set; }


   
    public ExpenseType ExpenseType { get; set; }

    
}