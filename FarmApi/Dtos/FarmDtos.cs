using System.ComponentModel.DataAnnotations;

namespace FarmApi.DTOs;

public class TransactionResponseDto
{
    public Guid TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? TypeCode { get; set; }
    public string? CategoryCode { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}

public class CreateTransactionRequestDto
{
    [Required]
    public DateTime TransactionDate { get; set; }
    [Required]
    public string? TypeCode { get; set; }
    [Required]
    public string? CategoryCode { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

public class GetOverviewFarmDashboardDto
{
    public decimal TotalIncome { get; set; }
    public decimal PercentageIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal PercentageExpense { get; set; }
    public decimal NetProfit { get; set; }
    public string? ActiveNetFlag { get; set; }
    public List<TransactionsListOverviewDto> TransactionsList { get; set; } = new();
    public List<BreakdownListOverviewDto> BreakdownList { get; set; } = new();

}

public class BreakdownListOverviewDto
{
    public string? CategoryCode { get; set; }
    public string? CategoryName { get; set; }
    public decimal Percentage { get; set; }
}

public class TransactionsListOverviewDto
{
    public Guid UniId { get; set; }
    public string? Date { get; set; }
    public string? CategoryCode { get; set; }
    public string? CategoryName { get; set; }
    public string? TypeCode { get; set; }
    public string? TypeName { get; set; }
    public decimal? Amount { get; set; }
}