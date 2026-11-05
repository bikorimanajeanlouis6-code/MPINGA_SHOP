using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class AddCustomerDTO
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Select the customer's sex.")]
    public string Sex { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain exactly 10 digits.")]
    public string PhoneNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}

public class GetCustomerDTO
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string TotalConsumed { get; set; } = "0";
    public string TotalPaid { get; set; } = "0";
    public string Balance { get; set; } = "0";
    public string Status { get; set; } = string.Empty;
    public string UserAdded { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}