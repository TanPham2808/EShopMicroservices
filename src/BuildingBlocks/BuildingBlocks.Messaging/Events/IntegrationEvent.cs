﻿namespace BuildingBlocks.Messaging.Events;

/// <summary>
/// Tích hợp Event
/// </summary>
public record IntegrationEvent
{
    public Guid Id => Guid.NewGuid();

    /// <summary>
    /// Thời điểm xảy ra sự kiện, giúp theo dõi và ghi nhật ký các sự kiện.
    /// </summary>
    public DateTime OccurredOn => DateTime.Now;

    /// <summary>
    ///  Loại sự kiện, giúp xác định loại sự kiện nào đã xảy ra, điều này có thể hữu ích trong việc xử lý sự kiện.
    /// </summary>
    public string EventType => GetType().AssemblyQualifiedName;
}
