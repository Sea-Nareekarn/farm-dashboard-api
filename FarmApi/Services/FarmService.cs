using FarmApi.DTOs;
using FarmApi.Models;
using FarmApi.Repositories;
using FarmApi.Exceptions;

namespace FarmApi.Services;

public class FarmService : IFarmService
{
    private readonly IFarmRepository _repository;

    public FarmService(IFarmRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetOverviewFarmDashboardDto> GetAllTransactionsAsync()
    {
        GetOverviewFarmDashboardDto response = new GetOverviewFarmDashboardDto();
        try
        {
            var transactions = await _repository.GetAllAsync();
            if (transactions == null || !transactions.Any())
            {
                throw new FarmServiceException("error-data-not-found", "No transactions found in the database.");
            }

            var masTypes = await _repository.GetMasTypeAsync();
            var getCategory = masTypes.Where(t => t.GroupCode == "FARMD_CATEGORY").ToList();
            var getType = masTypes.Where(t => t.GroupCode == "FARMD_TYPE").ToList();

            response.TransactionsList = transactions.OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionsListOverviewDto
            {
                UniId = t.TransactionId,
                Date = t.TransactionDate.ToString("yyyy-MM-dd"),
                TypeCode = t.TypeCode ?? string.Empty,
                TypeName = getType.FirstOrDefault(c => c.Code == t.TypeCode)?.NameLoc ?? string.Empty,
                CategoryCode = t.CategoryCode ?? string.Empty,
                CategoryName = getCategory.FirstOrDefault(c => c.Code == t.CategoryCode)?.NameLoc ?? string.Empty,
                Amount = t.Amount,
             }).ToList();


            var totalSum = transactions.Sum(t => t.Amount);
            
            response.TotalIncome = transactions.Where(t => t.TypeCode == "IN").Sum(t => t.Amount);
            response.TotalExpense = transactions.Where(t => t.TypeCode == "OUT").Sum(t => t.Amount);

            response.PercentageIncome = totalSum > 0 ? Math.Round(response.TotalIncome / totalSum * 100, 2) : 0;
            response.PercentageExpense = totalSum > 0 ? Math.Round(response.TotalExpense / totalSum * 100, 2) : 0;

            response.NetProfit = response.TotalIncome - response.TotalExpense;
            response.ActiveNetFlag = response.NetProfit >= 0 ? "Y" : "N";


            response.BreakdownList = transactions.GroupBy(t => t.CategoryCode)
                .Select(g => new BreakdownListOverviewDto
                {
                    CategoryCode = g.Key,
                    CategoryName = getCategory.FirstOrDefault(c => c.Code == g.Key)?.NameLoc ?? string.Empty,
                    Percentage = totalSum > 0 ? Math.Round(g.Sum(t => t.Amount) / totalSum * 100, 2) : 0
                }).ToList();

            return response;
        }
        catch (FarmServiceException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            throw new FarmServiceException("error-database-query", "Failed to retrieve transactions from the database.", ex);
        }
    }

    public async Task<TransactionResponseDto> CreateTransactionAsync(CreateTransactionRequestDto request, string userId)
    {
        var entity = new FarmDashboardTransactionDb
        {
            TransactionId = Guid.NewGuid(),
            TransactionDate = request.TransactionDate,
            TypeCode = request.TypeCode,
            CategoryCode = request.CategoryCode,
            Amount = request.Amount,
            Description = request.Description,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        return new TransactionResponseDto
        {
            TransactionId = entity.TransactionId,
            TransactionDate = entity.TransactionDate,
            TypeCode = entity.TypeCode,
            CategoryCode = entity.CategoryCode,
            Amount = entity.Amount,
            Description = entity.Description,
            CreatedAt = entity.UpdatedAt ?? entity.CreatedAt,
            CreatedBy = entity.UpdatedBy ?? entity.CreatedBy
        };
    }
}