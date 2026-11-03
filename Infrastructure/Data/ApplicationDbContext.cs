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
        ApplyUserAddedToNewEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyUserAddedToNewEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyUserAddedToNewEntities()
    {
        if (_userContext is not { IsAuthenticated: true }
            || string.IsNullOrWhiteSpace(_userContext.Email))
        {
            return;
        }

        foreach (var entry in ChangeTracker.Entries()
                     .Where(entry => entry.State == EntityState.Added
                         && entry.Metadata.FindProperty("UserAdded")?.ClrType == typeof(string)))
        {
            entry.Property("UserAdded").CurrentValue = _userContext.Email;
        }
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountDeposit> AccountDeposits { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Expenses> Expenses { get; set; }
    public DbSet<ExpenseType> ExpenseTypes { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Sales> Sales { get; set; }
    public DbSet<SaleItems> SalesItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Replenishment> Replenishments { get; set; }
    public DbSet<Stock> Stocks { get; set; }

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
