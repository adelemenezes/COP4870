using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System.Collections.ObjectModel;

namespace Maui.eCommerce.ViewModels
{
    public class MainViewModel
    {
        private readonly CommerceService _commerceService;

        public MainViewModel()
        {
            _commerceService = new CommerceService(
                ProductServiceProxy.Current,
                new CartService(ProductServiceProxy.Current)
            );
        }

        public string Display => "Hello World!";
        
        // For potential future use in ShoppingView
        public ObservableCollection<Product> CartItems => 
            new ObservableCollection<Product>(_commerceService.CartItems);
    }
}