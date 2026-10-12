using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.StockServices
{
    public class StockService : IStockService
    {
        private readonly IStock _stockRepository;

        public StockService(IStock stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public Task<List<GetStockDTO>> GetAllStocksAsync() =>
            _stockRepository.GetAllStockAsync();

        public Task<GetStockDTO> GetStockByIdAsync(int id) =>
            _stockRepository.GetStockByIdAsync(id);

        public Task AddStockAsync(AddStockDTO stock) =>
            _stockRepository.AddStockAsync(stock);
    }
}