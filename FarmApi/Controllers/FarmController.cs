using Microsoft.AspNetCore.Mvc;

namespace FarmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FarmController : ControllerBase
{
    // สั่งให้ทำงานเมื่อเรียก GET api/Farm/transactions
    [HttpGet("transactions")]
    public IActionResult GetTransactions()
    {
        // สร้างข้อมูลหลอกมาเทสก่อน
        var mockData = new[] {
            new { Id = 1, Item = "Eggs", Amount = 50 },
            new { Id = 2, Item = "Milk", Amount = 20 }
        };
        
        return Ok(mockData);
    }
}