namespace Catalog.API.Products.CreateProduct
{
    // record class chứa các request
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<CreateProductResult>;

    // record class chứa kết quả trả về
    public record CreateProductResult(Guid Id);

    public class CreateProductHandler (IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
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
