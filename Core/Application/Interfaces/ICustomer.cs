using Application.DTOs;

namespace Application.Interfaces;

public interface ICustomer
{
    Task<List<GetCustomerDTO>> GetAllCustomerAsync();
    Task<GetCustomerDTO> GetCustomerByIdAsync(int id);
    Task AddCustomerAsync(AddCustomerDTO customer);
}