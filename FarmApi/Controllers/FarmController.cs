using System.Security.Claims;
using FarmApi.DTOs;
using FarmApi.Services; 
using FarmApi.Extensions; 
using FarmApi.Exceptions; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 

namespace FarmApi.Controllers;

[ApiController]
[Authorize] // Bearer Token 
[Route("api/farm-dashboard")] 
public class FarmController : ControllerBase
{
    private readonly IFarmService _farmService;
    private readonly ILogger<FarmController> _logger; 
    private string UserId => User.GetUserId();


    public FarmController(IFarmService farmService, ILogger<FarmController> logger)
    {
        _farmService = farmService;
        _logger = logger;
    }

    //GET api/farm-dashboard/reports/overview
    [HttpGet("reports/overview")]
    public async Task<IActionResult> GetOverviewFarmDashboard()
    {
        try
        {
            GetOverviewFarmDashboardDto transactions = await _farmService.GetAllTransactionsAsync();
            return Ok(transactions);
        }
        catch (FarmServiceException ex) 
        {
            _logger.LogError(ex, "Error fetching transactions: {ErrorMessage}", ex.Message);
            return StatusCode(400, new ErrorResponseDto { Code = ex.ErrorCode, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching transactions."); 
            return StatusCode(500, new ErrorResponseDto { Code = "internal-server-error", Message = "An unexpected error occurred." });
        }
    }

    // //POST api/Farm/transactions
    [HttpPost("transactions")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequestDto request)
    {
        try
        {            
            var result = await _farmService.CreateTransactionAsync(request, UserId);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction.");
            return BadRequest(new ErrorResponseDto { Code = "transaction-creation-failed", Message = null }); // ใช้ ErrorResponseDto และ Message เป็น null
        }
    }
}
