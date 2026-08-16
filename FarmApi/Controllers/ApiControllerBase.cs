using FarmApi.DTOs;
using FarmApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FarmApi.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ILogger Logger;

    protected ApiControllerBase(ILogger logger)
    {
        Logger = logger;
    }

    protected async Task<IActionResult> ExecuteAsync(
        Func<Task<IActionResult>> action,
        string logContext,
        string unexpectedErrorCode,
        string? unexpectedErrorMessage = null,
        int unexpectedStatusCode = StatusCodes.Status500InternalServerError)
    {
        try
        {
            return await action();
        }
        catch (FarmServiceException ex)
        {
            Logger.LogError(ex, "{Context}: {ErrorMessage}", logContext, ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest,
                new ErrorResponseDto { Code = ex.ErrorCode, Message = ex.Message });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{Context}", logContext);
            return StatusCode(unexpectedStatusCode,
                new ErrorResponseDto { Code = unexpectedErrorCode, Message = unexpectedErrorMessage });
        }
    }
}
