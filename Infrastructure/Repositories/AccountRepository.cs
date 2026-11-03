using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            var entities = await _dbcontext.Accounts
                .AsNoTracking()
                .OrderByDescending(account => account.Id)
                .ToListAsync();
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
            if (account.Balance < 0)
                throw new ArgumentOutOfRangeException(nameof(account), "An opening balance cannot be negative.");

            await using var transaction = await _dbcontext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var entity = new Account
            {
                AccountName = account.AccountName,
                AccountNumber = account.AccountNumber,
                BankName = account.BankName,
                Balance = account.Balance,
                Status = account.Status,
                UserAdded = account.UserAdded,
                DateAdded = account.DateAdded
            };
            await _dbcontext.Accounts.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();

            if (entity.Balance > 0)
            {
                await _dbcontext.AccountDeposits.AddAsync(new AccountDeposit
                {
                    AccountId = entity.Id,
                    Amount = entity.Balance,
                    Description = "Opening balance",
                    DateAdded = entity.DateAdded
                });
                await _dbcontext.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }

        public async Task UpdateAccountBalanceAsync(int id, decimal balance)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "A valid account ID is required.");

            var account = await _dbcontext.Accounts
                .FirstOrDefaultAsync(existingAccount => existingAccount.Id == id)
                ?? throw new KeyNotFoundException($"Account with ID {id} was not found.");

            account.Balance = balance;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DepositIntoAccountAsync(int id, decimal amount, string description)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "A valid account ID is required.");
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "A deposit amount must be greater than zero.");
            if (amount != decimal.Round(amount, 2))
                throw new ArgumentOutOfRangeException(nameof(amount), "A deposit amount cannot have more than two decimal places.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A deposit description is required.", nameof(description));
            if (description.Trim().Length > 200)
                throw new ArgumentOutOfRangeException(nameof(description), "A deposit description cannot exceed 200 characters.");

            await using var transaction = await _dbcontext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var account = await _dbcontext.Accounts
                .FirstOrDefaultAsync(existingAccount => existingAccount.Id == id)
                ?? throw new KeyNotFoundException($"Account with ID {id} was not found.");

            account.Balance += amount;
            await _dbcontext.AccountDeposits.AddAsync(new AccountDeposit
            {
                AccountId = id,
                Amount = amount,
                Description = description.Trim(),
                DateAdded = DateTime.Now
            });
            await _dbcontext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<List<GetAccountDepositDTO>> GetAccountDepositsAsync(int accountId)
        {
            if (accountId <= 0)
                throw new ArgumentOutOfRangeException(nameof(accountId), "A valid account ID is required.");

            var deposits = await _dbcontext.AccountDeposits
                .AsNoTracking()
                .Where(deposit => deposit.AccountId == accountId)
                .OrderByDescending(deposit => deposit.DateAdded)
                .ThenByDescending(deposit => deposit.Id)
                .ToListAsync();

            return deposits.Select(deposit => new GetAccountDepositDTO
            {
                Id = deposit.Id,
                AccountId = deposit.AccountId,
                Amount = deposit.Amount,
                Description = deposit.Description,
                DateAdded = deposit.DateAdded,
                UserAdded = deposit.UserAdded
            }).ToList();
        }

        private static GetAccountDTO ToDto(Account entity) => new()
        {
            Id = entity.Id,
            AccountName = entity.AccountName,
            AccountNumber = entity.AccountNumber,
            BankName = entity.BankName,
            Balance = entity.Balance,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DateAdded
        };
    }
}
