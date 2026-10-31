using Application.DTOs;

namespace Application.Services.SaleItemsServices
{
    public interface ISaleItemsService
    {
           Task <List<GetSaleItemsDTO>>GetAllSaleItemsAsync();
           Task AddSaleItemsAsync(AddSaleItemsDTO saleItems); 
           Task AddSaleItemsAsync(List<AddSaleItemsDTO> saleItems);
           Task <GetSaleItemsDTO> GetSaleItemsByIdAsync(int id);
       
    }
}