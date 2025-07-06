using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Library.eCommerce.Services
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> CartItems { get; } = new();

        const float DEFAULT_TAX = 7f;
        public float TaxRate => DEFAULT_TAX;

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
            OnPropertyChanged(nameof(CartItems)); // Add this line
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
                return true; // No change needed
            }
            OnPropertyChanged(nameof(CartItems)); 
            return true;
        }

        public string Checkout()
        {
            if (!CartItems.Any()) return "Cart is empty.";

            var output = new System.Text.StringBuilder();
            output.AppendLine("********** CHECKOUT **********");
            
            double total = 0;
            int numberOfItems = 0;

            foreach (var p in CartItems)
            {
                output.AppendLine($"{p.Name}");
                output.AppendLine($"\t${p.Price:F2}");
                output.AppendLine($"\tQuantity: {p.Quantity}");
                total += p.Price * p.Quantity;
                numberOfItems += p.Quantity;
            }

            output.AppendLine("\n******************************");
            output.AppendLine($"\nNumber of items: {numberOfItems}");
            output.AppendLine($"Total Without tax: ${total:F2}");
            
            double tax = total * DEFAULT_TAX / 100;
            output.AppendLine($"Tax ({DEFAULT_TAX:F1}%): ${tax:F2}");
            output.AppendLine($"Total with tax: ${(total + tax):F2}");
            output.AppendLine("\nThank you for your order!");

            CartItems.Clear();
            return output.ToString();
        }
    }
}