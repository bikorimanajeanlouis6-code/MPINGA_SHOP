using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.SaleItemsServices
{
    public class SaleItemsService : ISaleItemsService
    {
        private readonly ISaleItems _saleItemsRepository;

        public SaleItemsService(ISaleItems saleItemsRepository)
        {
            _saleItemsRepository = saleItemsRepository;
        }

        public Task<List<GetSaleItemsDTO>> GetAllSaleItemsAsync() =>
            _saleItemsRepository.GetAllSaleItemsAsync();

        public Task<GetSaleItemsDTO> GetSaleItemsByIdAsync(int id) =>
            _saleItemsRepository.GetSaleItemsByIdAsync(id);

        public Task AddSaleItemsAsync(AddSaleItemsDTO saleItems) =>
            _saleItemsRepository.AddSaleItemsAsync(saleItems);

        public Task AddSaleItemsAsync(List<AddSaleItemsDTO> saleItems) =>
            _saleItemsRepository.AddSaleItemsAsync(saleItems);
    }
}