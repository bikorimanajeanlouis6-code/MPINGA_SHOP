using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.PaymentMethordServices
{
    public class PaymentMethordService : IPaymentMethordService
    {
        private readonly IPaymentMethord _paymentMethodRepository;

        public PaymentMethordService(IPaymentMethord paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public Task<List<GetPaymentMethordDTO>> GetAllPaymentMethordAsync() =>
            _paymentMethodRepository.GetAllPaymentMethordAsync();

        public Task<GetPaymentMethordDTO> GetPaymentMethordByIdAsync(int id) =>
            _paymentMethodRepository.GetPaymentMethordByIdAsync(id);

        public Task AddPaymentMethordAsync(AddPaymentMethordDTO paymentMethord) =>
            _paymentMethodRepository.AddPaymentMethordAsync(paymentMethord);
    }
}