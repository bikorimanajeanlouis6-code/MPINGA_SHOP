using Application.DTOs;

namespace Application.Services.ProductServices
{
    public interface IProductService
    {
           Task <List<GetProductDTO>>GetAllProductAsync();
           Task AddProductAsync(AddProductDTO product); 
           Task <GetProductDTO> GetProductByIdAsync(int id);
       
    }
}