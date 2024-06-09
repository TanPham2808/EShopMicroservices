using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extentions
{
    /// <summary>
    /// Phương thức cấu hình Masstransit với RabbitMQ
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        //  Đăng ký MassTransit vào dịch vụ Dependency Injection và cấu hình nó.
        services.AddMassTransit(config =>
        {
            // Thiết lập định dạng tên endpoint theo kiểu Kebab case (ví dụ: my-endpoint-name)
            config.SetKebabCaseEndpointNameFormatter();

            // Nếu assembly khác null, đăng ký tất cả các consumer trong assembly đó với MassTransit.
            if (assembly != null)
                config.AddConsumers(assembly);

            // Cấu hình MassTransit để sử dụng RabbitMQ.
            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });

                // Tự động cấu hình các endpoint dựa trên các consumer đã được đăng ký.
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
