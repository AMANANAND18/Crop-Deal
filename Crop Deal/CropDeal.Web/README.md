# CropDeal (Option B)

ASP.NET Core MVC application using MySQL and 3-tier architecture.

## Architecture

- Presentation Layer: MVC Controllers (`Controllers`)
- Business Layer: Services (`Services`)
- Data Layer: Repositories + EF Core (`Repositories`, `Data`)

## Tech

- ASP.NET Core 8
- Entity Framework Core
- MySQL (Pomelo provider)

## Implemented modules

- Account: register/login for Farmer, Dealer, Admin
- Farmer: publish crop, view own listings
- Dealer: view open listings, create order, pay
- Admin: dashboard report, order list

## Setup

1. Install .NET 8 SDK.
2. Create MySQL database: `cropdeal_db`.
3. Update connection string in `appsettings.json`.
4. From project folder:

```bash
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

## Notes

- Payment gateway is mocked in `PaymentService`.
- This is a solid base to add receipts/invoice PDF generation, Facebook OAuth, subscriptions, notifications, and advanced reporting.
