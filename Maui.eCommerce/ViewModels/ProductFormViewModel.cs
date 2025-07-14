using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Maui.eCommerce.ViewModels
{
    public class ProductFormViewModel : INotifyPropertyChanged
    {
        // Services
        private readonly ICommerceService _commerceService;
        private Product? _editingProduct;
        // Properties
        private string _name = string.Empty;
        private string _quantity = string.Empty;
        private string _price = string.Empty;
        private string _rating = string.Empty;

        public ProductFormViewModel(ICommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            private set => SetField(ref _isEditMode, value);
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public string Quantity
        {
            get => _quantity;
            set => SetField(ref _quantity, value);
        }

        public string Price
        {
            get => _price;
            set => SetField(ref _price, value);
        }

        public string Rating
        {
            get => _rating;
            set => SetField(ref _rating, value);
        }

        public void LoadProduct(long productId)
        {
            IsEditMode = true;
            _editingProduct = _commerceService.GetProduct(productId);
            if (_editingProduct != null)
            {
                Name = _editingProduct.Name;
                Quantity = _editingProduct.Quantity.ToString();
                Price = _editingProduct.Price.ToString("F2");
                Rating = _editingProduct.Rating.ToString();
            }
        }

        public void ResetForm()
        {
            IsEditMode = false;
            _editingProduct = null;
            Name = string.Empty;
            Quantity = string.Empty;
            Price = string.Empty;
            Rating = string.Empty;
        }

        public bool SaveProduct()
        {
            if (!ValidateInputs()) return false;

            if (IsEditMode && _editingProduct != null)
            {
                // Update existing product
                return _commerceService.UpdateItem(_editingProduct.ID, product =>
                {
                    product.Name = Name;
                    product.Quantity = int.Parse(Quantity);
                    product.Price = double.Parse(Price);
                    product.Rating = int.Parse(Rating);
                });
            }
            else
            {
                return _commerceService.CreateItem(
                    Name,
                    int.Parse(Quantity),
                    double.Parse(Price),
                    int.Parse(Rating));
            }
        }

        private bool ValidateInputs()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && int.TryParse(Quantity, out int qty) && qty >= 0
                && double.TryParse(Price, out double price) && price >= 0
                && int.TryParse(Rating, out int rating) && rating is >= 1 and <= 5;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}