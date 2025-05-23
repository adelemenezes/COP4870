using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.eCommerce.Services
{
    public class CommerceService : ICommerceService
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public List<Product> CartItems => _cartService.CartItems;

        public CommerceService(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        // Transition implementations
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

        public bool RemoveItem(long id) => RemoveProduct(id);
        
        public string CheckOut() => Checkout();

        // Interface implementations
        public Product? GetProduct(long id) => _productService.GetProduct(id);
        public IEnumerable<Product> GetAllProducts() => _productService.GetAllProducts();
        public Product AddProduct(Product product) => _productService.AddProduct(product);
        public bool RemoveProduct(long id) => _productService.RemoveProduct(id);
        public long GetNextProductId() => _productService.GetNextProductId();
        public bool AddToCart(long productId) => _cartService.AddToCart(productId);
        public bool RemoveFromCart(long productId) => _cartService.RemoveFromCart(productId);
        public bool UpdateCartItemQuantity(long productId, int newQuantity) => 
            _cartService.UpdateCartItemQuantity(productId, newQuantity);
        public string Checkout() => _cartService.Checkout();

        private string FormatProductList(IEnumerable<Product> products)
        {
            return !products.Any() 
                ? "List is empty." 
                : string.Join("\n", products.Select(p => p.ToString()));
        }
    }
}