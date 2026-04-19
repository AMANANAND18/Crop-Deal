using CropDeal.Web.Data;
using CropDeal.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CropDeal.Web.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetByRoleAsync(UserRole role);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}

public interface ICropRepository
{
    Task AddAsync(CropListing listing);
    Task<List<CropListing>> GetOpenListingsAsync();
    Task<List<CropListing>> GetByFarmerIdAsync(int farmerId);
    Task<CropListing?> GetByIdAsync(int id);
    Task UpdateAsync(CropListing listing);
}

public interface IOrderRepository
{
    Task AddOrderAsync(PurchaseOrder order);
    Task AddPaymentAsync(PaymentTransaction transaction);
    Task<PurchaseOrder?> GetOrderByIdAsync(int id);
    Task<List<PurchaseOrder>> GetOrdersAsync();
    Task UpdateOrderAsync(PurchaseOrder order);
}

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email) =>
        db.Users.FirstOrDefaultAsync(x => x.Email == email);

    public Task<User?> GetByIdAsync(int id) =>
        db.Users.FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<User>> GetByRoleAsync(UserRole role) =>
        db.Users.Where(x => x.Role == role).ToListAsync();

    public async Task AddAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }
}

public class CropRepository(AppDbContext db) : ICropRepository
{
    public async Task AddAsync(CropListing listing)
    {
        db.CropListings.Add(listing);
        await db.SaveChangesAsync();
    }

    public Task<List<CropListing>> GetOpenListingsAsync() =>
        db.CropListings.Where(x => !x.IsSold)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();

    public Task<List<CropListing>> GetByFarmerIdAsync(int farmerId) =>
        db.CropListings.Where(x => x.FarmerId == farmerId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();

    public Task<CropListing?> GetByIdAsync(int id) =>
        db.CropListings.FirstOrDefaultAsync(x => x.Id == id);

    public async Task UpdateAsync(CropListing listing)
    {
        db.CropListings.Update(listing);
        await db.SaveChangesAsync();
    }
}

public class OrderRepository(AppDbContext db) : IOrderRepository
{
    public async Task AddOrderAsync(PurchaseOrder order)
    {
        db.PurchaseOrders.Add(order);
        await db.SaveChangesAsync();
    }

    public async Task AddPaymentAsync(PaymentTransaction transaction)
    {
        db.PaymentTransactions.Add(transaction);
        await db.SaveChangesAsync();
    }

    public Task<PurchaseOrder?> GetOrderByIdAsync(int id) =>
        db.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<PurchaseOrder>> GetOrdersAsync() =>
        db.PurchaseOrders.OrderByDescending(x => x.CreatedAtUtc).ToListAsync();

    public async Task UpdateOrderAsync(PurchaseOrder order)
    {
        db.PurchaseOrders.Update(order);
        await db.SaveChangesAsync();
    }
}
