namespace CropDeal.Web.Models;

public enum UserRole
{
    Farmer = 1,
    Dealer = 2,
    Admin = 3
}

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public string? BankAccountNumber { get; set; }
    public string? IfscCode { get; set; }
}

public class CropListing
{
    public int Id { get; set; }
    public int FarmerId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CropType { get; set; } = string.Empty;
    public decimal QuantityInKg { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal? ExpectedPricePerKg { get; set; }
    public bool IsSold { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

public class PurchaseOrder
{
    public int Id { get; set; }
    public int CropListingId { get; set; }
    public int FarmerId { get; set; }
    public int DealerId { get; set; }
    public decimal FinalPricePerKg { get; set; }
    public decimal FinalQuantityInKg { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "PendingPayment";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

public class PaymentTransaction
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = "Card";
    public string Status { get; set; } = "Success";
    public string TransactionReference { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
