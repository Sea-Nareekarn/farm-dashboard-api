using FarmApi.DTOs;
using FarmApi.Services; 
using FarmApi.Extensions; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmApi.Controllers;

[ApiController]
[Authorize] // Bearer Token 
[Route("api/farm-dashboard")] 
public class FarmController : ApiControllerBase
{
    private readonly IFarmService _farmService;
    private string UserId => User.GetUserId();


    public FarmController(IFarmService farmService, ILogger<FarmController> logger)
        : base(logger)
    {
        _farmService = farmService;
    }

    //GET api/farm-dashboard/reports/overview
    [HttpGet("reports/overview")]
    public Task<IActionResult> GetOverviewFarmDashboard()
    {
        return ExecuteAsync(async () =>
        {
            GetOverviewFarmDashboardDto transactions = await _farmService.GetAllTransactionsAsync();
            return Ok(transactions);
        },
        logContext: "Error fetching transactions",
        unexpectedErrorCode: "internal-server-error",
        unexpectedErrorMessage: "An unexpected error occurred.");
    }

    // //POST api/Farm/transactions
    [HttpPost("transactions")]
    public Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequestDto request)
    {
        return ExecuteAsync(async () =>
        {
            var result = await _farmService.CreateTransactionAsync(request, UserId);
            return Ok(result);
        },
        logContext: "Error creating transaction",
        unexpectedErrorCode: "transaction-creation-failed",
        unexpectedStatusCode: StatusCodes.Status400BadRequest);
    }
}
