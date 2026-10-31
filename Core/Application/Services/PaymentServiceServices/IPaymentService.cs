using Application.DTOs;

namespace Application.Services.PaymentServices
{
    public interface IPaymentService
    {
           Task <List<GetPaymentDTO>>GetAllPaymentAsync();
           Task AddPaymentAsync(AddPaymentDTO payment); 
           Task <GetPaymentDTO> GetPaymentByIdAsync(int id);
       
    }
}