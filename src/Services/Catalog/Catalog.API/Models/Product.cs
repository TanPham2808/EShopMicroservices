namespace Catalog.API.Models;

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!; // Giá trị Name ko thể null (cú pháp C# ver 8.0)
        public List<string> Category { get; set; } = new();
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; }
    }


