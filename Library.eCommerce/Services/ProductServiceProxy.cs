using System;
using System.Collections.Generic;
using System.Linq;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy : IProductService
    {
        private static ProductServiceProxy? _instance;
        private static readonly object _instanceLock = new();

        private static CartService? _cartService;
        private static readonly object _cartServiceLock = new();

        public List<Product> Products { get; private set; }

        private long _highestIdUsed = 0;

        private ProductServiceProxy()
        {
            Products = new List<Product>
            {
                new Product { ID = 1, Name = "Product1", Quantity = 10, Price = 100.0, Rating = 5 },
                new Product { ID = 2, Name = "Product2" },
                new Product { ID = 3, Name = "Product3" },
                new Product { ID = 4, Name = "Product4" }
            };
        }

        public static ProductServiceProxy Current
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new ProductServiceProxy();
                    return _instance;
                }
            }
        }

        public static CartService CartService
        {
            get
            {
                lock (_cartServiceLock)
                {
                    _cartService ??= new CartService(Current);
                    return _cartService;
                }
            }
        }

        public long GetNextProductId()
        {
            lock (_instanceLock)
            {
                return ++_highestIdUsed;
            }
        }

        public Product AddProduct(Product product)
        {
            lock (_instanceLock)
            {
                Products.Add(product);
                return product;
            }
        }

        public bool RemoveProduct(long id)
        {
            lock (_instanceLock)
            {
                return Products.RemoveAll(p => p.ID == id) > 0;
            }
        }

        public Product? GetProduct(long id)
        {
            lock (_instanceLock)
            {
                return Products.FirstOrDefault(p => p.ID == id);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            lock (_instanceLock)
            {
                return Products.ToList();
            }
        }
    }
}
