using Library.eCommerce.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Library.eCommerce.Interfaces
{
    public interface ICartService
    {
        ObservableCollection<Product> CartItems { get; }
        bool AddToCart(long productId);
        bool RemoveFromCart(long productId);
        bool UpdateCartItemQuantity(long productId, int newQuantity);
        string Checkout();
    }
}