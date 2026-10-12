namespace Application.DTOs;

public class AddExpenseDTO
{
    public int Id { get; set; }
    public string ExpenseName { get; set; } = string.Empty;
    public decimal ExpenseAmount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string ExpenseDescription { get; set; } = string.Empty;
    public int ExpenseTypeId { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetExpenseDTO
{
    public int Id { get; set; }
    public string ExpenseName { get; set; } = string.Empty;
    public decimal ExpenseAmount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string ExpenseTypeName { get; set; } = string.Empty;
    public string ExpenseDescription { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public string UserAdded { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}