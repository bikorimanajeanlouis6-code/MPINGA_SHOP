using Application.Services.AccountServices;
using Application.Services.CustomerServices;
using Application.Services.ExpenseServices;
using Application.Services.ExpenseTypeServices;
using Application.Services.PaymentMethordServices;
using Application.Services.PaymentServices;
using Application.Services.ProductCategoryServices;
using Application.Services.ProductServices;
using Application.Services.ReplenishmentServices;
using Application.Services.SaleItemsServices;
using Application.Services.SalesServices;
using Application.Services.StockServices;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependanceInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IExpenseService, ExpenseService>();
        services.AddTransient<IExpenseTypeService, ExpenseTypeService>();
        services.AddTransient<IPaymentService, PaymentService>();
        services.AddTransient<IPaymentMethordService, PaymentMethordService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IProductCategoryService, ProductCategoryService>();
        services.AddTransient<IReplenishmentService, ReplenishmentService>();
        services.AddTransient<ISaleItemsService, SaleItemsService>();
        services.AddTransient<ISalesService, SalesService>();
        services.AddTransient<IStockService, StockService>();

        return services;
    }
}