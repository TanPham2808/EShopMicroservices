using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate : IEntity
{
    /// <summary>
    /// Một danh sách chỉ đọc các sự kiện miền liên quan đến Aggregate
    /// </summary>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Một phương thức để xóa và trả về các sự kiện miền hiện tại trong Aggregate
    /// </summary>
    /// <returns></returns>
    IDomainEvent[] ClearDomainEvents();
}

