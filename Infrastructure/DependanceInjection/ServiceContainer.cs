using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependanceInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "The 'DefaultConnection' connection string is not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddAuthenticationService(configuration);
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IIdentity, IdentityRepository>();
        services.AddScoped<IAccount, AccountRepository>();
        services.AddScoped<ICustomer, CustomerRepository>();
        services.AddScoped<IExpense, ExpenseRepository>();
        services.AddScoped<IExpenseType, ExpenseTypeRepository>();
        services.AddScoped<IPayment, PaymentRepository>();
        services.AddScoped<IPaymentMethord, PaymentMethordRepository>();
        services.AddScoped<IProduct, ProductRepository>();
        services.AddScoped<IProductCategory, ProductCategoryRepository>();
        services.AddScoped<IReplenishment, ReplenishmentRepository>();
        services.AddScoped<ISaleItems, SaleItemsRepository>();
        services.AddScoped<ISales, SalesRepository>();
        services.AddScoped<IStock, StockRepository>();

        return services;
    }
}