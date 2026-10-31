using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccount
    {
        private readonly ApplicationDbContext _dbcontext;

        public AccountRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetAccountDTO>> GetAllAccountAsync()
        {
            var entities = await _dbcontext.Accounts.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetAccountDTO> GetAccountByIdAsync(int id)
        {
            var entity = await _dbcontext.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(account => account.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Account with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddAccountAsync(AddAccountDTO account)
        {
            ArgumentNullException.ThrowIfNull(account);
            await _dbcontext.Accounts.AddAsync(new Account
            {
                AccountName = account.AccountName,
                AccountNumber = account.AccountNumber,
                BankName = account.BankName,
                Status = account.Status,
                UserAdded = account.UserAdded,
                DateAdded = account.DateAdded
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetAccountDTO ToDto(Account entity) => new()
        {
            Id = entity.Id,
            AccountName = entity.AccountName,
            AccountNumber = entity.AccountNumber,
            BankName = entity.BankName,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DateAdded
        };
    }
}
