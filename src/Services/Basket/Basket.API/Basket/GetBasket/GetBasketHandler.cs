using Basket.API.Models;
using BuildingBlocks.CQRS;

namespace Basket.API.Basket.GetBasket;

// Request query
public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

// Response 
public record GetBasketResult(ShoppingCart Cart);

public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        // Get basket from Database
        //var basket = await repository.GetBasket(query.UserName);
        return new GetBasketResult(new ShoppingCart("tp"));
    }
}

