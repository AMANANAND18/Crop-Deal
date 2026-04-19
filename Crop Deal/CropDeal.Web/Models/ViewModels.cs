namespace CropDeal.Web.Models;

public class RegisterViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}

public class LoginViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class PublishCropViewModel
{
    public int FarmerId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CropType { get; set; } = string.Empty;
    public decimal QuantityInKg { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal? ExpectedPricePerKg { get; set; }
}

public class CreateOrderViewModel
{
    public int CropListingId { get; set; }
    public int FarmerId { get; set; }
    public int DealerId { get; set; }
    public decimal QuantityKg { get; set; }
    public decimal PricePerKg { get; set; }
}

public class PayOrderViewModel
{
    public int OrderId { get; set; }
    public string Method { get; set; } = "Card";
}
