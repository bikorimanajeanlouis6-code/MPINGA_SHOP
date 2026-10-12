using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomer _customerRepository;

        public CustomerService(ICustomer customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<List<GetCustomerDTO>> GetAllCustomerAsync() =>
            _customerRepository.GetAllCustomerAsync();

        public Task<GetCustomerDTO> GetCustomerByIdAsync(int id) =>
            _customerRepository.GetCustomerByIdAsync(id);

        public Task AddCustomerAsync(AddCustomerDTO customer) =>
            _customerRepository.AddCustomerAsync(customer);
    }
}