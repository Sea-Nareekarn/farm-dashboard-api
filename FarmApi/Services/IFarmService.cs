using FarmApi.DTOs;

namespace FarmApi.Services;

public interface IFarmService
{
    Task<GetOverviewFarmDashboardDto> GetAllTransactionsAsync();
    Task<TransactionResponseDto> CreateTransactionAsync(CreateTransactionRequestDto request, string userId);
}