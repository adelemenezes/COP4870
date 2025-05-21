using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Library.eCommerce.Interfaces;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // raise the event
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products)); // notify the view that the products have changed
        }
        public InventoryManagementViewModel()
        {
            // constructor
            // can put any initialization code here
        }

        // this is the property that will be bound to the view
        // it is a list of products
        // can put any property
        public ObservableCollection<Product> Products { // matched binding with this name
            get
            {
                return new ObservableCollection<Product>(_svc.Products);
            }
        }

        public Product? SelectedProduct { get; set; }
        private ProductServiceProxy _svc = ProductServiceProxy.Current;

        public Product? Delete() // we already know what to delete because it is stored in selected product
        {
            if (SelectedProduct == null)
            {
                return null;
            }

            var deletedProduct = SelectedProduct; // saves the old product
            _svc.RemoveProduct(SelectedProduct.ID);
            
            SelectedProduct = null;
            NotifyPropertyChanged(nameof(Products)); // notify the view that the products have changed
            
            return deletedProduct; // in case they want to undo
        }
    }
}