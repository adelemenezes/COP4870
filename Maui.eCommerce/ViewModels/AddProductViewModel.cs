using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Maui.eCommerce.ViewModels
{
    public class AddProductViewModel : INotifyPropertyChanged
    {
        private readonly ICommerceService _commerceService;
        
        // Product properties with basic validation
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private string _quantity = "1";
        public string Quantity
        {
            get => _quantity;
            set => SetField(ref _quantity, value);
        }

        private string _price = "0.00";
        public string Price
        {
            get => _price;
            set => SetField(ref _price, value);
        }
        private string _rating = "1";
        public string Rating
        {
            get => _rating;
            set => SetField(ref _rating, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public AddProductViewModel(ICommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        public bool AddProduct()
        {
            if (int.TryParse(Quantity, out int qty) && 
                double.TryParse(Price, out double price) &&
                int.TryParse(Rating, out int rating))
            {
                return _commerceService.CreateItem(Name, qty, price, rating);
            }
            return false;
        }

        // Helper method for property changes
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}