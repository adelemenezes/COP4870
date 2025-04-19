using Library.eCommerce.Models;
using System.Collections.Generic;

namespace Library.eCommerce.Interfaces
{
    public interface ICartService
    {
        List<Product> CartItems { get; }
        bool AddToCart(long productId);
        bool RemoveFromCart(long productId);
        bool UpdateCartItemQuantity(long productId, int newQuantity);
        string Checkout();
    }
}