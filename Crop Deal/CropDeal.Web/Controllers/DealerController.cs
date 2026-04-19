using CropDeal.Web.Models;
using CropDeal.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CropDeal.Web.Controllers;

[Route("dealer")]
public class DealerController(ICropService cropService, IOrderService orderService, IPaymentService paymentService) : Controller
{
    [HttpGet("open-crops")]
    public async Task<IActionResult> OpenCrops()
    {
        var listings = await cropService.GetOpenListingsAsync();
        return Ok(listings);
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderViewModel model)
    {
        var order = await orderService.CreateOrderAsync(
            model.CropListingId,
            model.FarmerId,
            model.DealerId,
            model.QuantityKg,
            model.PricePerKg);

        return Ok(order);
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PayOrderViewModel model)
    {
        var result = await paymentService.MarkPaidAsync(model.OrderId, model.Method);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }
}
