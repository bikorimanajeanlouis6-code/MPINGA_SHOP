using Application.DTOs;

namespace Application.Interfaces;

public interface IPaymentMethord
{
    Task<List<GetPaymentMethordDTO>> GetAllPaymentMethordAsync();
    Task<GetPaymentMethordDTO> GetPaymentMethordByIdAsync(int id);
    Task AddPaymentMethordAsync(AddPaymentMethordDTO paymentMethord);
}