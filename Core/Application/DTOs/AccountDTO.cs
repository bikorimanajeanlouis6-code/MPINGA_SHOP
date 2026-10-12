namespace Application.DTOs;

public class AddAccountDTO
{
    public int Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}

public class GetAccountDTO
{
    public int Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}