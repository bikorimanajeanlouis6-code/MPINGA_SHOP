using Application.DTOs;

namespace Application.Services.ReplenishmentServices
{
    public interface IReplenishmentService
    {
           Task <List<GetReplenishmentDTO>>GetAllReplenishmentsAsync();
           Task AddReplenishmentAsync(AddReplenishmentDTO replenishment); 
           Task <GetReplenishmentDTO> GetReplenishmentByIdAsync(int id);
       
    }
}