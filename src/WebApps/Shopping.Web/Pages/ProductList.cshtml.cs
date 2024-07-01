using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Models.Catalog;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class ProductListModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly ILogger<ProductListModel> _logger;

        public ProductListModel(ICatalogService catalogService, IBasketService basketService, ILogger<ProductListModel> logger)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _logger = logger;
        }

        public IEnumerable<string> CategoryList { get; set; } = [];
        public IEnumerable<ProductModel> ProductList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var response = await _catalogService.GetProducts();

            CategoryList = response.Products.SelectMany(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                ProductList = response.Products.Where(p => p.Category.Contains(categoryName));
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = response.Products;
            }

            return Page();
        }

        /// <summary>
        /// V?i thu?c tính asp-page-handler="AddToCart" và ph??ng th?c POST, 
        /// Razor Pages s? tìm ph??ng th?c handler có tên OnPostAddToCartAsync
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            _logger.LogInformation("Add to cart button clicked");
            var productResponse = await _catalogService.GetProduct(productId);

            var basket = await _basketService.LoadUserBasket();

            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Product.Name,
                Price = productResponse.Product.Price,
                Quantity = 1,
                Color = "Black"
            });

            await _basketService.StoreBasket(new StoreBasketRequest(basket));

            return RedirectToPage("Cart");
        }
    }
}

