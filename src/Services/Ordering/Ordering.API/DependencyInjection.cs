using BuildingBlocks.Exceptions.CustomException;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();  // Tùy chỉnh Exception Response (Nhớ add thêm app.UseExceptionHandler)

        // Thêm tính năng kiểm tra tình trạng sức khỏe của services
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Database")!); 

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseExceptionHandler(options => { });  // Hanlde tùy chỉnh Exception Response (Đi 1 cặp chung với method AddExceptionHandler)

        // Kích hoạt tính năng kiểm tra tình trạng sức khỏe của services
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }); 
        return app;
    }
}
