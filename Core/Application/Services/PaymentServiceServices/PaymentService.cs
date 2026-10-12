using Application.DTOs;

namespace Application.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        public Task<List<GetPaymentDTO>> GetAllPaymentAsync()
        {
            return Task.FromResult(new List<GetPaymentDTO>());
        }

        public Task<GetPaymentDTO> GetPaymentByIdAsync(int id)
        {
            return Task.FromResult(new GetPaymentDTO());
        }

        public Task AddPaymentAsync(AddPaymentDTO payment)
        {
            return Task.CompletedTask;
        }
    }
}