using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IPayment _paymentRepository;

        public PaymentService(IPayment paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Task<List<GetPaymentDTO>> GetAllPaymentAsync() =>
            _paymentRepository.GetAllPaymentAsync();

        public Task<GetPaymentDTO> GetPaymentByIdAsync(int id) =>
            _paymentRepository.GetPaymentByIdAsync(id);

        public Task AddPaymentAsync(AddPaymentDTO payment) =>
            _paymentRepository.AddPaymentAsync(payment);
    }
}