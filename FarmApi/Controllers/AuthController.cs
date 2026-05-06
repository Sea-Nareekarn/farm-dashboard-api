using System.ComponentModel.DataAnnotations;
using FarmApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FarmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Supabase.Client _supabaseClient;
    private readonly ILogger<AuthController> _logger;

    public AuthController(Supabase.Client supabaseClient, ILogger<AuthController> logger)
    {
        _supabaseClient = supabaseClient;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try 
        {
            var session = await _supabaseClient.Auth.SignInWithPassword(request.Email, request.Password);
            
            if (session != null)
            {
                return Ok(new { token = session.AccessToken });
            }
            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for email: {Email}", request.Email);
            return BadRequest(new { message = "Login failed. Please check your credentials." });
        }
    }
}