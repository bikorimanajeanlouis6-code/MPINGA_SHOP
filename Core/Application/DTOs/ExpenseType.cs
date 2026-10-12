namespace Application.DTOs;

public class AddExpenseTypeDTO
{
    public int Id { get; set; }

    public string ExpenseTypeName { get; set; } = string.Empty;
    public string ExpenseTypeDescription { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}

public class GetExpenseTypeDTO
{
    public int Id { get; set; }
    public string ExpenseTypeName { get; set; } = string.Empty;
    public string ExpenseTypeDescription { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}