using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;

namespace Ordering.Application.Order.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string Name) 
    : IQuery<GetOrdersByNameResult>;

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
