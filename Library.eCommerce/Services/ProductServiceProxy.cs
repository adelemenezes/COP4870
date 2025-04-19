using System;
using System.Collections.Generic;
using System.Linq;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy : IProductService
    {
        private ProductServiceProxy()
        {
            Products = new List<Product>();
        }

        private static ProductServiceProxy? instance;
        private static readonly object instanceLock = new();

        public static ProductServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        instance ??= new ProductServiceProxy();
                    }
                }
                return instance;
            }
        }

        public List<Product> Products { get; private set; }

        public long GetNextProductId()
        {
            lock (instanceLock)
            {
                return Products.Any() ? Products.Max(p => p.ID) + 1 : 1;
            }
        }

        public Product AddProduct(Product product)
        {
            lock (instanceLock)
            {
                Products.Add(product);
                return product;
            }
        }

        public bool RemoveProduct(long id)
        {
            lock (instanceLock)
            {
                return Products.RemoveAll(p => p.ID == id) > 0;
            }
        }

        public Product? GetProduct(long id)
        {
            lock (instanceLock)
            {
                return Products.FirstOrDefault(p => p.ID == id);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            lock (instanceLock)
            {
                return Products.ToList();
            }
        }
    }
}