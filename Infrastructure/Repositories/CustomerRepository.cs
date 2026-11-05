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
            var entities = await _dbcontext.Customers
                .AsNoTracking()
                .Include(customer => customer.Sales.Where(sale => sale.Status == "Active"))
                    .ThenInclude(sale => sale.Payments.Where(payment => payment.Status == "Active"))
                .OrderByDescending(customer => customer.Id)
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetCustomerDTO> GetCustomerByIdAsync(int id)
        {
            var entity = await _dbcontext.Customers.AsNoTracking()
                .Include(customer => customer.Sales.Where(sale => sale.Status == "Active"))
                    .ThenInclude(sale => sale.Payments.Where(payment => payment.Status == "Active"))
                .FirstOrDefaultAsync(customer => customer.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Customer with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddCustomerAsync(AddCustomerDTO customer)
        {
            ArgumentNullException.ThrowIfNull(customer);
            var phoneNumber = customer.PhoneNumber?.Trim() ?? string.Empty;
            if (phoneNumber.Length != 10 || phoneNumber.Any(character => character is < '0' or > '9'))
                throw new ArgumentException("Phone number must contain exactly 10 digits.", nameof(customer));

            if (await _dbcontext.Customers
                .AnyAsync(existingCustomer => existingCustomer.PhoneNumber.Trim() == phoneNumber))
            {
                throw new InvalidOperationException("A customer with this phone number is already registered.");
            }

            await _dbcontext.Customers.AddAsync(new Customer
            {
                Name = customer.CustomerName,
                Sex = customer.Sex,
                Email = customer.Email,
                PhoneNumber = phoneNumber,
                Status = customer.Status,
                UserAdded = customer.UserAdded,
                DateAdded = customer.DateAdded
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetCustomerDTO ToDto(Customer entity)
        {
            var totalConsumed = entity.Sales.Sum(sale => RepositoryValueConversions.ToDecimal(sale.TotalConsumed));
            var totalPaid = entity.Sales.Sum(sale => Math.Max(
                RepositoryValueConversions.ToDecimal(sale.TotalPaid),
                sale.Payments.Sum(payment => RepositoryValueConversions.ToDecimal(payment.PaymentAmount))));

            return new GetCustomerDTO
            {
                Id = entity.Id,
                CustomerName = entity.Name,
                Sex = entity.Sex,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                TotalConsumed = RepositoryValueConversions.ToStorageString(totalConsumed),
                TotalPaid = RepositoryValueConversions.ToStorageString(totalPaid),
                Balance = RepositoryValueConversions.ToStorageString(totalConsumed - totalPaid),
                Status = entity.Status,
                UserAdded = entity.UserAdded,
                DateAdded = entity.DateAdded
            };
        }
    }
}
