using FarmApi.Models;

namespace FarmApi.Repositories;

public interface IFarmRepository
{
    Task<List<FarmDashboardTransactionDb>> GetAllAsync();
    Task<List<MasTypeDb>> GetMasTypeAsync();
    Task AddAsync(FarmDashboardTransactionDb transaction);
}