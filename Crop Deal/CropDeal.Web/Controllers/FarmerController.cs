using CropDeal.Web.Models;
using CropDeal.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CropDeal.Web.Controllers;

[Route("farmer")]
public class FarmerController(ICropService cropService) : Controller
{
    [HttpPost("publish-crop")]
    public async Task<IActionResult> PublishCrop([FromBody] PublishCropViewModel model)
    {
        var listing = new CropListing
        {
            FarmerId = model.FarmerId,
            Category = model.Category,
            CropType = model.CropType,
            QuantityInKg = model.QuantityInKg,
            Location = model.Location,
            ExpectedPricePerKg = model.ExpectedPricePerKg
        };

        await cropService.PublishAsync(listing);
        return Ok("Crop listing published successfully.");
    }

    [HttpGet("{farmerId:int}/listings")]
    public async Task<IActionResult> MyListings(int farmerId)
    {
        var listings = await cropService.GetFarmerListingsAsync(farmerId);
        return Ok(listings);
    }
}
