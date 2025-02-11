using Pizza_Star.Models;

namespace Pizza_Star.Interfaces
{
    public interface ICart
    {
        string ShopCartId { get; set; }
        List<ShopCartItem> ShopCartItems { get; set; }
        Task<int> GetShopCartItemsCountAsync();
        Task<IEnumerable<ShopCartItem>> GetShopCartItemsAsync();
        Task<ShopCartItem> GetShopCartItemAsync(int shopCartItemId);
        Task AddToCartAsync(Product product, int quantity);
        Task RemoveFromCartAsync(ShopCartItem shopCartItem);
        Task UpdateFromCartAsync(ShopCartItem shopCartItem);
        Task ClearCartAsync();
    }
}
