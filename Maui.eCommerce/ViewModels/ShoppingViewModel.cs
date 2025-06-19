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

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Product> CartItems { get; } = new ObservableCollection<Product>();

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
            
            // Initialize cart
            RefreshCart();
            
            // Subscribe to cart changes if supported
            if (_cartService is INotifyPropertyChanged notifier)
            {
                notifier.PropertyChanged += OnCartServicePropertyChanged;
            }
        }

        private void OnCartServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ICartService.CartItems))
            {
                RefreshCart();
            }
        }

        public void RefreshCart()
        {
            CartItems.Clear();
            foreach (var item in _cartService.CartItems)
            {
                CartItems.Add(item);
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
            var result = _cartService.RemoveFromCart(productId);
            // No need to manually refresh here - the PropertyChanged event will trigger it
            return result;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Clean up event subscription when view model is disposed
        public void CleanUp()
        {
            if (_cartService is INotifyPropertyChanged notifier)
            {
                notifier.PropertyChanged -= OnCartServicePropertyChanged;
            }
        }
    }
}