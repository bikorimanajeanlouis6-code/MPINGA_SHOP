using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.ReplenishmentServices
{
    public class ReplenishmentService : IReplenishmentService
    {
        private readonly IReplenishment _replenishmentRepository;

        public ReplenishmentService(IReplenishment replenishmentRepository)
        {
            _replenishmentRepository = replenishmentRepository;
        }

        public Task<List<GetReplenishmentDTO>> GetAllReplenishmentsAsync() =>
            _replenishmentRepository.GetAllReplenishmentsAsync();

        public Task<GetReplenishmentDTO> GetReplenishmentByIdAsync(int id) =>
            _replenishmentRepository.GetReplenishmentByIdAsync(id);

        public Task AddReplenishmentAsync(AddReplenishmentDTO replenishment) =>
            _replenishmentRepository.AddReplenishmentAsync(replenishment);
    }
}