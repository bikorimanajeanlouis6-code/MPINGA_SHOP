using Application.DTOs;

namespace Application.Services.CustomerServices
{
    public interface ICustomerService
    {
           Task <List<GetCustomerDTO>>GetAllCustomerAsync();
           Task AddCustomerAsync(AddCustomerDTO customer); 
           Task <GetCustomerDTO> GetCustomerByIdAsync(int id);
       
    }
}