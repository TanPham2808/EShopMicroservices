using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        // Tạo một scope dịch vụ mới (service scope) để quản lý vòng đời của các dịch vụ
        using var scope = app.Services.CreateScope();

        // Lấy đối tượng ApplicationDbContext từ dịch vụ trong scope hiện tại
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Thực hiện di chuyển (migrate) cơ sở dữ liệu lên phiên bản mới nhất
        context.Database.MigrateAsync().GetAwaiter().GetResult();

        // Gọi hàm SeedAsync để khởi tạo dữ liệu mẫu vào cơ sở dữ liệu (nếu cần)
        await SeedAsync(context);
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        await SeedProductAsync(context);
        await SeedOrdersWithItemsAsync(context);
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        // Kiểm tra có data chưa
        if (!await context.Customers.AnyAsync())
        {
            // Add data Customer mẫu
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProductAsync(ApplicationDbContext context)
    {
        // Kiểm tra có data chưa
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
    {
        // Kiểm tra có data chưa
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }
}
