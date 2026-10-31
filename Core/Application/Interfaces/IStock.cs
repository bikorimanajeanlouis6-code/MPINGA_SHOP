using Application.DTOs;

namespace Application.Interfaces;

public interface IStock
{
    Task<List<GetStockDTO>> GetAllStockAsync();
    Task<GetStockDTO> GetStockByIdAsync(int id);
    Task AddStockAsync(AddStockDTO stock);
}