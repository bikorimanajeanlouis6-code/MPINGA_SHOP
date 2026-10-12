using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.SalesServices
{
    public class SalesService : ISalesService
    {
        private readonly ISales _salesRepository;

        public SalesService(ISales salesRepository)
        {
            _salesRepository = salesRepository;
        }

        public Task<List<GetSalesDTO>> GetAllSalesAsync() =>
            _salesRepository.GetAllSalesAsync();

        public Task<GetSalesDTO> GetSalesByIdAsync(int id) =>
            _salesRepository.GetSalesByIdAsync(id);

        public Task AddSalesAsync(AddSalesDTO sales) =>
            _salesRepository.AddSalesAsync(sales);
    }
}