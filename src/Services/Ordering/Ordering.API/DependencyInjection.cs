using BuildingBlocks.Exceptions.CustomException;
using Carter;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();  // Tùy chỉnh Exception Response (Nhớ add thêm app.UseExceptionHandler)

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseExceptionHandler(options => { });  // Hanlde tùy chỉnh Exception Response (Đi 1 cặp chung với method AddExceptionHandler)

        return app;
    }
}
