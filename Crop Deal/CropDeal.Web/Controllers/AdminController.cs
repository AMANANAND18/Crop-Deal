using CropDeal.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CropDeal.Web.Controllers;

[Route("admin")]
public class AdminController(IReportService reportService, IOrderService orderService) : Controller
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var report = await reportService.GetDashboardAsync();
        return Ok(report);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> Orders()
    {
        var orders = await orderService.GetAllOrdersAsync();
        return Ok(orders);
    }
}
