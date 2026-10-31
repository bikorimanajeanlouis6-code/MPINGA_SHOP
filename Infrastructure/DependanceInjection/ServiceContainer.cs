using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependanceInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MPINGASHOPCON")
            ?? throw new InvalidOperationException(
                "The 'MPINGASHOPCON' connection string is not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

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
