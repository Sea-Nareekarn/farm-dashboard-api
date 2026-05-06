using FarmApi.Data;
using FarmApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmApi.Repositories;

public class FarmRepository : IFarmRepository
{
    private readonly FarmDbContext _context;

    public FarmRepository(FarmDbContext context)
    {
        _context = context;
    }

    public async Task<List<FarmDashboardTransactionDb>> GetAllAsync()
    {
        return await _context.FarmDashboardTransactionDb.ToListAsync();
    }

    public async Task<List<MasTypeDb>> GetMasTypeAsync()
    {
        return await _context.MasTypeDb.ToListAsync();
    }

    public async Task AddAsync(FarmDashboardTransactionDb transaction)
    {
        await _context.FarmDashboardTransactionDb.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
}