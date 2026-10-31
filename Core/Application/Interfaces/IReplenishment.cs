using Application.DTOs;

namespace Application.Interfaces;

public interface IReplenishment
{
    Task<List<GetReplenishmentDTO>> GetAllReplenishmentsAsync();
    Task<GetReplenishmentDTO> GetReplenishmentByIdAsync(int id);
    Task AddReplenishmentAsync(AddReplenishmentDTO replenishment);
}