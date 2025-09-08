using System.ComponentModel;
using Library.eCommerce.Interfaces;

namespace Maui.eCommerce.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly ICartService _cartService;

        public float TaxRate
        {
            get => _cartService.TaxRate;
            set
            {
                _cartService.TaxRate = value;
                OnPropertyChanged(nameof(TaxRate));
            }
        }

        public SettingsViewModel(ICartService cartService)
        {
            _cartService = cartService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}