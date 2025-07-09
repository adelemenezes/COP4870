using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized; 
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
            _cartService.CartItems.CollectionChanged += CartItems_CollectionChanged;
            
            ((INotifyPropertyChanged)_cartService).PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == nameof(ICartService.TaxRate))
                    UpdateTotal();
            };
            
            foreach (var item in _cartService.CartItems)
            {
                item.PropertyChanged += CartItem_PropertyChanged;
            }
            
            UpdateTotal();
        }

        private void CartItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Product item in e.OldItems)
                {
                    item.PropertyChanged -= CartItem_PropertyChanged;
                }
            }
            
            if (e.NewItems != null)
            {
                foreach (Product item in e.NewItems)
                {
                    item.PropertyChanged += CartItem_PropertyChanged;
                }
            }
            
            UpdateTotal();
        }

        private void CartItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateTotal();
        }


        private void UpdateTotal()
        {
            TotalText = _cartService.GetTotalText();
            TaxText = _cartService.GetTaxText();
            TotalWithTaxText = _cartService.GetTotalWithTaxText();
        }


        public bool RemoveFromCart(long productId)
        {
            bool removeSuccess = _cartService.RemoveFromCart(productId);
            UpdateTotal(); // Force immediate total refresh
            return removeSuccess;
        }

        private string _taxText = "Tax (7%): $0.00";
        public string TaxText
        {
            get => _taxText;
            set => SetField(ref _taxText, value);
        }

        private string _totalWithTaxText = "Total: $0.00";
        public string TotalWithTaxText
        {
            get => _totalWithTaxText;
            set => SetField(ref _totalWithTaxText, value);
        }

        // Helper method for property changes
        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName); // Changed from OnPropertyChanged
            return true;
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