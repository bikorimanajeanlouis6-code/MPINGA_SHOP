using Application.DTOs;

namespace Application.Interfaces;

public interface ISaleItems
{
    Task<List<GetSaleItemsDTO>> GetAllSaleItemsAsync();
    Task<GetSaleItemsDTO> GetSaleItemsByIdAsync(int id);
    Task AddSaleItemsAsync(AddSaleItemsDTO saleItems);
    Task AddSaleItemsAsync(List<AddSaleItemsDTO> saleItems);
}