using Application.DTOs;

namespace Application.Services.PaymentMethordServices
{
    public class PaymentMethordService : IPaymentMethordService
    {
        public Task<List<GetPaymentMethordDTO>> GetAllPaymentMethordAsync()
        {
            return Task.FromResult(new List<GetPaymentMethordDTO>());
        }

        public Task<GetPaymentMethordDTO> GetPaymentMethordByIdAsync(int id)
        {
            return Task.FromResult(new GetPaymentMethordDTO());
        }

        public Task AddPaymentMethordAsync(AddPaymentMethordDTO paymentMethord)
        {
            return Task.CompletedTask;
        }
    }
}