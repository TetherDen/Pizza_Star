using Microsoft.AspNetCore.Mvc;
using Pizza_Star.Interfaces;

namespace Pizza_Star.ViewComponents
{
    // компонент представления для отображения 8 случайных продуктов в представлении...
    public class RelatedProductsViewComponent : ViewComponent   
    {
        private readonly IProduct _products;

        public RelatedProductsViewComponent(IProduct products)
        {
            _products = products;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            return View("RelatedProducts", await _products.GetEightRandomProductsAsync(productId));
        }
    }
}
