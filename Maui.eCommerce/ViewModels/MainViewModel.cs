using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System.Collections.ObjectModel;

namespace Maui.eCommerce.ViewModels
{
    public class MainViewModel
    {
        private readonly CommerceService _commerceService;
        private readonly CartService _cartService; // i will initialize _commerceService in the constructor

        public MainViewModel()
        {
            _cartService = ProductServiceProxy.CartService; // Use singleton
            _commerceService = new CommerceService(
                ProductServiceProxy.Current,
                _cartService
            );
        }

        public string Display => "Thanks for shopping with us!";
        public ObservableCollection<Product> CartItems => new ObservableCollection<Product>(_commerceService.CartItems);
        public CommerceService CommerceService => _commerceService;
        public CartService CartService => _cartService;
    }
}