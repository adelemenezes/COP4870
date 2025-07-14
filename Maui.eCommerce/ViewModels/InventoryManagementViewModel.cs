using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;
using Microsoft.Maui.ApplicationModel;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        private readonly ICommerceService _commerceService;
        private ObservableCollection<Product>? _products;
        public event PropertyChangedEventHandler? PropertyChanged;
        public Product? SelectedProduct { get; set; }

        public InventoryManagementViewModel(ICommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        public ObservableCollection<Product> Products =>
            _products ??= new ObservableCollection<Product>(_commerceService.GetAllProducts());

        public Product? Delete()
        {
            if (SelectedProduct == null) return null;

            var deletedProduct = SelectedProduct;
            _commerceService.RemoveProduct(SelectedProduct.ID);
            RefreshProductList();
            SelectedProduct = null;

            return deletedProduct;
        }

        public bool DeleteProduct(Product? product = null)
        {
            var productToDelete = product ?? SelectedProduct;
            if (productToDelete == null) return false;
            bool removeSuccess = _commerceService.RemoveFromCart(productToDelete.ID);
            bool inventoryRemoved = _commerceService.RemoveProduct(productToDelete.ID);

            RefreshProductList();

            if (productToDelete == SelectedProduct)
            {
                SelectedProduct = null;
            }

            return inventoryRemoved;
        }

        public void AddToCart(Product? product = null)
        {
            var productToAdd = product ?? SelectedProduct;
            if (productToAdd == null) return;

            _commerceService.AddToCart(productToAdd.ID);
            RefreshProductList();

            if (productToAdd == SelectedProduct)
            {
                SelectedProduct = null;
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshProductList()
        {
            _products = null;
            NotifyPropertyChanged(nameof(Products));
        }
    }
}