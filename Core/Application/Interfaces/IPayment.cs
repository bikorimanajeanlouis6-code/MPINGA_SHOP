using Application.DTOs;

namespace Application.Interfaces;

public interface IPayment
{
    Task<List<GetPaymentDTO>> GetAllPaymentAsync();
    Task<GetPaymentDTO> GetPaymentByIdAsync(int id);
    Task AddPaymentAsync(AddPaymentDTO payment);
}