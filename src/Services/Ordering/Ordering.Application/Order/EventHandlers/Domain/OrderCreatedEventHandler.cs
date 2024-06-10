using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Order.EventHandlers.Domain;

public class OrderCreatedEventHandler
    (   IPublishEndpoint publishEndpoint,  // Sử dụng để bắn message vào RabbitMQ
        IFeatureManager featureManager,    // Sử dụng FeatureManager để bật tắt tính năng mong muốn (using Microsoft.FeatureManagement)
        ILogger<OrderCreatedEventHandler> logger)
        :
        INotificationHandler<OrderCreatedEvent> 
{
    /// <summary>
    /// Handler sau khi Order được publish
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
        
        // Nếu True sẽ enable tính năng này lên (cờ này nằm trong file appsetting.json)
        // Bắn message queue đi qua services khác sau khi tạo đơn hàng thành công.
        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
    }
}
