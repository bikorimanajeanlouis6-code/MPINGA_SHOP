using Application.DTOs;

namespace Application.Interfaces;

public interface IProduct
{
    Task<List<GetProductDTO>> GetAllProductAsync();
    Task<GetProductDTO> GetProductByIdAsync(int id);
    Task AddProductAsync(AddProductDTO product);
}