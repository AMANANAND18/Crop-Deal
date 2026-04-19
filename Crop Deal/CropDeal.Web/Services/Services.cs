using System.Security.Cryptography;
using System.Text;
using CropDeal.Web.Models;
using CropDeal.Web.Repositories;

namespace CropDeal.Web.Services;

public interface IAuthService
{
    Task<(bool Success, string Message)> RegisterAsync(string fullName, string email, string password, UserRole role);
    Task<User?> LoginAsync(string email, string password);
}

public interface ICropService
{
    Task PublishAsync(CropListing listing);
    Task<List<CropListing>> GetOpenListingsAsync();
    Task<List<CropListing>> GetFarmerListingsAsync(int farmerId);
}

public interface IOrderService
{
    Task<PurchaseOrder> CreateOrderAsync(int cropListingId, int farmerId, int dealerId, decimal quantityKg, decimal pricePerKg);
    Task<List<PurchaseOrder>> GetAllOrdersAsync();
}

public interface IPaymentService
{
    Task<(bool Success, string Message)> MarkPaidAsync(int orderId, string method);
}

public interface IReportService
{
    Task<object> GetDashboardAsync();
}

public class AuthService(IUserRepository userRepository) : IAuthService
{
    public async Task<(bool Success, string Message)> RegisterAsync(string fullName, string email, string password, UserRole role)
    {
        if (await userRepository.GetByEmailAsync(email) is not null)
            return (false, "Email already registered.");

        var user = new User
        {
            FullName = fullName,
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = Hash(password),
            Role = role
        };

        await userRepository.AddAsync(user);
        return (true, "Registration completed.");
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await userRepository.GetByEmailAsync(email.Trim().ToLowerInvariant());
        if (user is null || user.PasswordHash != Hash(password) || !user.IsActive)
            return null;

        return user;
    }

    private static string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}

public class CropService(ICropRepository cropRepository) : ICropService
{
    public Task PublishAsync(CropListing listing) => cropRepository.AddAsync(listing);
    public Task<List<CropListing>> GetOpenListingsAsync() => cropRepository.GetOpenListingsAsync();
    public Task<List<CropListing>> GetFarmerListingsAsync(int farmerId) => cropRepository.GetByFarmerIdAsync(farmerId);
}

public class OrderService(ICropRepository cropRepository, IOrderRepository orderRepository) : IOrderService
{
    public async Task<PurchaseOrder> CreateOrderAsync(int cropListingId, int farmerId, int dealerId, decimal quantityKg, decimal pricePerKg)
    {
        var listing = await cropRepository.GetByIdAsync(cropListingId) ?? throw new InvalidOperationException("Listing not found.");
        if (listing.IsSold)
            throw new InvalidOperationException("Crop already sold.");

        listing.IsSold = true;
        await cropRepository.UpdateAsync(listing);

        var order = new PurchaseOrder
        {
            CropListingId = cropListingId,
            FarmerId = farmerId,
            DealerId = dealerId,
            FinalQuantityInKg = quantityKg,
            FinalPricePerKg = pricePerKg,
            TotalAmount = quantityKg * pricePerKg,
            Status = "PendingPayment"
        };

        await orderRepository.AddOrderAsync(order);
        return order;
    }

    public Task<List<PurchaseOrder>> GetAllOrdersAsync() => orderRepository.GetOrdersAsync();
}

public class PaymentService(IOrderRepository orderRepository) : IPaymentService
{
    public async Task<(bool Success, string Message)> MarkPaidAsync(int orderId, string method)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            return (false, "Order not found.");

        if (order.Status == "Paid")
            return (false, "Order already paid.");

        var transaction = new PaymentTransaction
        {
            PurchaseOrderId = order.Id,
            Amount = order.TotalAmount,
            Method = method,
            Status = "Success"
        };

        await orderRepository.AddPaymentAsync(transaction);
        order.Status = "Paid";
        await orderRepository.UpdateOrderAsync(order);
        return (true, "Payment successful and receipt/invoice can be generated.");
    }
}

public class ReportService(IUserRepository userRepository, ICropRepository cropRepository, IOrderRepository orderRepository) : IReportService
{
    public async Task<object> GetDashboardAsync()
    {
        var farmers = await userRepository.GetByRoleAsync(UserRole.Farmer);
        var dealers = await userRepository.GetByRoleAsync(UserRole.Dealer);
        var listings = await cropRepository.GetOpenListingsAsync();
        var orders = await orderRepository.GetOrdersAsync();

        return new
        {
            TotalFarmers = farmers.Count,
            TotalDealers = dealers.Count,
            OpenListings = listings.Count,
            TotalOrders = orders.Count,
            PaidOrders = orders.Count(x => x.Status == "Paid")
        };
    }
}
