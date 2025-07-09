using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Library.eCommerce.Services
{
    public class CartService : ICartService, INotifyPropertyChanged
    {
        private readonly IProductService _productService;

        public ObservableCollection<Product> CartItems { get; } = new();

        private float _taxRate = 7.0f;
        public float TaxRate
        {
            get => _taxRate;
            set
            {
                if (Math.Abs(_taxRate - value) < 0.1f) return;
                _taxRate = value;
                OnPropertyChanged();
            }
        }

        public CartService(IProductService productService)
        {
            _productService = productService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool AddToCart(long productId)
        {
            var product = _productService.GetProduct(productId);
            if (product == null || product.Quantity < 1) return false;

            var cartItem = CartItems.FirstOrDefault(p => p.ID == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new Product(product) { Quantity = 1 };
                CartItems.Add(cartItem);
            }

            product.Quantity--;
            OnPropertyChanged(nameof(CartItems));
            return true;
        }

        public bool RemoveFromCart(long productId)
        {
            var cartItem = CartItems.FirstOrDefault(p => p.ID == productId);
            var product = _productService.GetProduct(productId);

            if (cartItem == null || product == null) return false;

            product.Quantity += cartItem.Quantity;
            CartItems.Remove(cartItem);
            OnPropertyChanged(nameof(CartItems));

            return true;
        }

        public bool UpdateCartItemQuantity(long productId, int newQuantity)
        {
            var cartItem = CartItems.FirstOrDefault(p => p.ID == productId);
            var product = _productService.GetProduct(productId);

            if (cartItem == null || product == null) return false;

            int oldQuantity = cartItem.Quantity;

            if (oldQuantity > newQuantity)
            {
                cartItem.Quantity = newQuantity;
                product.Quantity += oldQuantity - newQuantity;

                if (cartItem.Quantity == 0)
                {
                    CartItems.Remove(cartItem);
                }
            }
            else if (oldQuantity < newQuantity)
            {
                if (newQuantity - oldQuantity > product.Quantity)
                {
                    return false;
                }

                product.Quantity -= newQuantity - oldQuantity;
                cartItem.Quantity = newQuantity;
            }
            else
            {
                return true;
            }

            OnPropertyChanged(nameof(CartItems));
            return true;
        }

        public (double subtotal, double tax, double total) CalculateTotals()
        {
            double subtotal = Math.Round(CartItems.Sum(item => item.Price * item.Quantity), 2);
            double tax = subtotal * Math.Round(TaxRate, 1) / 100;
            double total = Math.Round(subtotal + tax, 2);

            return (subtotal, tax, total);
        }

        public string GetTotalText()
        {
            var (subtotal, _, _) = CalculateTotals();
            return $"Subtotal: ${subtotal:F2}";
        }

        public string GetTaxText()
        {
            var (_, tax, _) = CalculateTotals();
            return $"Tax ({TaxRate:F1}%): ${tax:F2}";
        }

        public string GetTotalWithTaxText()
        {
            var (_, _, total) = CalculateTotals();
            return $"Total: ${total:F2}";
        }

        public string Checkout()
        {
            if (!CartItems.Any()) return "Cart is empty.";

            var (subtotal, tax, total) = CalculateTotals();
            int numberOfItems = CartItems.Sum(p => p.Quantity);

            var output = new System.Text.StringBuilder();

            output.AppendLine("********** CHECKOUT **********");

            foreach (var p in CartItems)
            {
                output.AppendLine($"{p.Name}");
                output.AppendLine($"\t${p.Price:F2}");
                output.AppendLine($"\tQuantity: {p.Quantity}");
            }

            output.AppendLine("\n******************************");
            output.AppendLine($"\nNumber of items: {numberOfItems}");
            output.AppendLine($"Total Without tax: ${subtotal:F2}");
            output.AppendLine($"Tax ({TaxRate:F1}%): ${tax:F2}");
            output.AppendLine($"Total with tax: ${total:F2}");
            output.AppendLine("\nThank you for your order!");

            CartItems.Clear();
            return output.ToString();
        }
    }
}
