using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Library.eCommerce.Services
{
    public class CommerceService : ICommerceService
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public float TaxRate
        {
            get => _cartService.TaxRate;
            set => _cartService.TaxRate = value;
        }

        public ObservableCollection<Product> CartItems => _cartService.CartItems;

        public CommerceService(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        // Transition implementations
        public string GetTotalText() => _cartService.GetTotalText();
        public string GetTaxText() => _cartService.GetTaxText();
        public string GetTotalWithTaxText() => _cartService.GetTotalWithTaxText();
        public string ReadInventory() => FormatProductList(GetAllProducts());
        public string ReadCart() => FormatProductList(CartItems);

        public bool CreateItem(string name, int quantity, double price, int rating)
        {
            var product = new Product(name, quantity, price, rating, GetNextProductId());
            return AddProduct(product) != null;
        }

        public bool UpdateItem(long id, Action<Product> updateAction)
        {
            var product = GetProduct(id);
            if (product == null) return false;

            updateAction(product);
            return true;
        }

        public bool UpdateItemAmount(long id, int newQuantity) =>
            UpdateCartItemQuantity(id, newQuantity);

        public bool RemoveItem(long id) =>
            RemoveProduct(id);

        public string CheckOut() =>
            Checkout();

        // IProductService implementation
        public Product? GetProduct(long id) =>
            _productService.GetProduct(id);

        public IEnumerable<Product> GetAllProducts() =>
            _productService.GetAllProducts();

        public Product AddProduct(Product product) =>
            _productService.AddProduct(product);

        public bool RemoveProduct(long id) =>
            _productService.RemoveProduct(id);

        public long GetNextProductId() =>
            _productService.GetNextProductId();

        // ICartService implementation
        public bool AddToCart(long productId) =>
            _cartService.AddToCart(productId);

        public bool RemoveFromCart(long productId) =>
            _cartService.RemoveFromCart(productId);

        public bool UpdateCartItemQuantity(long productId, int newQuantity) =>
            _cartService.UpdateCartItemQuantity(productId, newQuantity);
        public IEnumerable<Product> GetSortedProducts(SortOption sortBy)
        {
            return _productService.GetSortedProducts(sortBy);
        }

        public void SortCartItems(SortOption sortBy)
        {
            _cartService.SortCartItems(sortBy);
        }

        // Modify the SortItems method to not use Products directly
        public void SortItems(SortOption sortBy, bool isCart)
        {
            if (isCart)
            {
                _cartService.SortCartItems(sortBy);
            }
            else
            {
                // Get sorted products and update the inventory
                var sortedProducts = _productService.GetSortedProducts(sortBy).ToList();
                // Remove all products and re-add them in sorted order
                foreach (var product in _productService.GetAllProducts().ToList())
                {
                    _productService.RemoveProduct(product.ID);
                }
                foreach (var product in sortedProducts)
                {
                    _productService.AddProduct(product);
                }
            }
        }
        public string Checkout() =>
            _cartService.Checkout();

        private string FormatProductList(IEnumerable<Product> products)
        {
            return !products.Any()
                ? "List is empty."
                : string.Join("\n", products.Select(p => p.ToString()));
        }
    }
}
