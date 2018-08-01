using Microsoft.AspNetCore.Mvc;
using Almacen.Models.Data;

namespace Almacen.Models.ViewModels
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;

        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke() => View(cart);
    }
}
