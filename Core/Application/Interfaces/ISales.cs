using Application.DTOs;

namespace Application.Interfaces;

public interface ISales
{
    Task<List<GetSalesDTO>> GetAllSalesAsync();
    Task<GetSalesDTO> GetSalesByIdAsync(int id);
    Task AddSalesAsync(AddSalesDTO sales);
}