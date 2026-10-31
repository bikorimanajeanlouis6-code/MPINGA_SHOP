using Application.DTOs;

namespace Application.Services.PaymentMethordServices
{
    public interface IPaymentMethordService
    {
           Task <List<GetPaymentMethordDTO>>GetAllPaymentMethordAsync();
           Task AddPaymentMethordAsync(AddPaymentMethordDTO paymentMethord); 
           Task <GetPaymentMethordDTO> GetPaymentMethordByIdAsync(int id);
       
    }
}