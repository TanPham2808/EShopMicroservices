

namespace Catalog.API.Products.CreateProduct
{
    // record class chứa các request
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<CreateProductResult>;

    // record class chứa kết quả trả về
    public record CreateProductResult(Guid Id);

    // Fluent Validate
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    public class CreateProductHandler 
        (IDocumentSession session, 
         IValidator<CreateProductCommand> validator) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Validate data input
            var result = await validator.ValidateAsync(command, cancellationToken);
            var errors = result.Errors.Select(x=>x.ErrorMessage).ToList();
            if(errors.Any())
            {
                throw new ValidationException(errors.FirstOrDefault());
            }

            //create Product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            //save to database (sủ dụng Marten Lib)
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            //return result

            return new CreateProductResult(product.Id);
        }
    }
}
