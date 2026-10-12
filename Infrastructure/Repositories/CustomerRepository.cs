using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomer
    {
        private readonly ApplicationDbContext _dbcontext;

        public CustomerRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetCustomerDTO>> GetAllCustomerAsync()
        {
            var entities = await _dbcontext.Customers.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetCustomerDTO> GetCustomerByIdAsync(int id)
        {
            var entity = await _dbcontext.Customers.AsNoTracking()
                .FirstOrDefaultAsync(customer => customer.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Customer with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddCustomerAsync(AddCustomerDTO customer)
        {
            ArgumentNullException.ThrowIfNull(customer);
            await _dbcontext.Customers.AddAsync(new Customer
            {
                Name = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Status = customer.Status,
                UserAdded = customer.UserAdded,
                DateAdded = customer.DateAdded
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetCustomerDTO ToDto(Customer entity) => new()
        {
            Id = entity.Id,
            CustomerName = entity.Name,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DateAdded
        };
    }
}
