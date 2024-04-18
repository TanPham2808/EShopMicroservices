using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket
{
    // Request command
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

    // Response
    public record StoreBasketResult(string UserName);

    // Validate
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class StoreBasketCommandHandler 
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.Cart;

            //TODO: store basket in database (use Marten upsert - if exist = update, if not exist = insert)
            //TODO: update cache
            //await repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult("tan pham is success");
        }
    }
}
