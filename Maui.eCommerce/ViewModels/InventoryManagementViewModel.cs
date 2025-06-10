using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        private readonly ICommerceService _commerceService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public InventoryManagementViewModel(ICommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        public ObservableCollection<Product> Products => 
            new ObservableCollection<Product>(_commerceService.GetAllProducts());

        public Product? SelectedProduct { get; set; }

        public Product? Delete()
        {
            if (SelectedProduct == null) return null;

            var deletedProduct = SelectedProduct;
            _commerceService.RemoveProduct(SelectedProduct.ID);
            
            SelectedProduct = null;
            NotifyPropertyChanged(nameof(Products));
            
            return deletedProduct;
        }

        public void DeleteProduct(Product? product = null)
        {
            var productToDelete = product ?? SelectedProduct;
            if (productToDelete == null) return;

            _commerceService.RemoveProduct(productToDelete?.ID ?? 0);
            
            if (productToDelete == SelectedProduct)
            {
                SelectedProduct = null;
            }
            
            NotifyPropertyChanged(nameof(Products));
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
        }
    }
}