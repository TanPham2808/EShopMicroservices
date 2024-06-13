using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddFeatureManagement();

        // Đăng ký RabbitMQ (Message Broker) Services
        // Consumer: Là thành phần của ứng dụng nhận message từ RabbitMQ.
        // Consumer kết nối tới RabbitMQ, subscribe vào các queue để nhận rồi xử lý các message.
        // Do trong OrderAPI có tính năng nhận queue để xử lý, nên cần thêm Assembly.GetExecutingAssembly()
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}

