using Microsoft.AspNetCore.Mvc;

namespace CropDeal.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => Content("CropDeal MVC 3-tier backend is running.");
}
