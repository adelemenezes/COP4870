using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingViewModel : INotifyPropertyChanged
    {
        private readonly ICartService _cartService;

        // Directly expose the service's collection
        public ObservableCollection<Product> CartItems => _cartService.CartItems;

        private string _totalText = "Total: $0.00";
        public string TotalText
        {
            get => _totalText;
            set
            {
                _totalText = value;
                NotifyPropertyChanged();
            }
        }

        public ShoppingViewModel(ICartService cartService)
        {
            _cartService = cartService;

            // Listen for cart changes to update total
            if (_cartService is INotifyPropertyChanged notifier)
            {
                notifier.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(ICartService.CartItems))
                    {
                        UpdateTotal();
                    }
                };
            }
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            var total = _cartService.CartItems.Sum(item => item.Price * item.Quantity);
            TotalText = $"Total: ${total:F2}";
        }

        public bool RemoveFromCart(long productId)
        {
            bool removeSuccess = _cartService.RemoveFromCart(productId);
            UpdateTotal(); // Force immediate total refresh
            return removeSuccess;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public string Checkout()
        {
            return _cartService.Checkout();
        }
    }
}