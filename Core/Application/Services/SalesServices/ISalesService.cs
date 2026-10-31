using Application.DTOs;

namespace Application.Services.SalesServices
{
    public interface ISalesService
    {
           Task <List<GetSalesDTO>>GetAllSalesAsync();
           Task AddSalesAsync(AddSalesDTO sales); 
           Task <GetSalesDTO> GetSalesByIdAsync(int id);
       
    }
}