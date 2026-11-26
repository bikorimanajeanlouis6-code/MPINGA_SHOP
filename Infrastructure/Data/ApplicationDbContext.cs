using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    private readonly IUserContext? _userContext;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IUserContext userContext)
        : base(options)
    {
        _userContext = userContext;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditFields();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyAuditFields()
    {
        if (_userContext is not { IsAuthenticated: true }
            || string.IsNullOrWhiteSpace(_userContext.Email))
        {
            return;
        }

        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added))
        {
            var userAddedProp = entry.Metadata.FindProperty("UserAdded");
            if (userAddedProp?.ClrType == typeof(string))
            {
                entry.Property("UserAdded").CurrentValue = _userContext.Email;
            }
        }
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountDeposit> AccountDeposits => Set<AccountDeposit>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Expenses> Expenses => Set<Expenses>();
    public DbSet<ExpenseType> ExpenseTypes => Set<ExpenseType>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Sales> Sales => Set<Sales>();
    public DbSet<SaleItems> SalesItems => Set<SaleItems>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Replenishment> Replenishments => Set<Replenishment>();
    public DbSet<Stock> Stocks => Set<Stock>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<IdentityRole<int>>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        builder.Entity<IdentityUserRole<int>>()
            .ToTable("UserRoles")
            .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

        builder.Entity<Account>()
            .Property(account => account.Balance)
            .HasPrecision(18, 2);

        builder.Entity<AccountDeposit>()
            .Property(deposit => deposit.Amount)
            .HasPrecision(18, 2);

        builder.Entity<AccountDeposit>()
            .HasOne(deposit => deposit.Account)
            .WithMany(account => account.Deposits)
            .HasForeignKey(deposit => deposit.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}