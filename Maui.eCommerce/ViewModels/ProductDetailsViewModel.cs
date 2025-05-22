using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Maui.eCommerce.ViewModels
{
    public class ProductDetailsViewModel : INotifyPropertyChanged
    {
        private readonly ICommerceService _commerceService;
        private string? _name;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ProductDetailsViewModel(ICommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool AddProduct()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            return _commerceService.CreateItem(Name!, 0, 0.0, 1); // Using default values for quantity/price/rating
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}