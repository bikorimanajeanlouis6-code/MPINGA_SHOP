using Application.DTOs;

namespace Application.Services.StockServices
{
    public interface IStockService
    {
           Task <List<GetStockDTO>>GetAllStocksAsync();
           Task AddStockAsync(AddStockDTO stock); 
           Task <GetStockDTO> GetStockByIdAsync(int id);
       
    }
}