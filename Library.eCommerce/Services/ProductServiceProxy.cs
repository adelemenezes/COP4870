using System;
using System.Collections.Generic;
using System.Linq;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy : IProductService
    {
        private static ProductServiceProxy? instance;
        private static readonly object instanceLock = new();
        public List<Product> Products { get; private set; }
        private long _highestIdUsed = 0;
        private ProductServiceProxy()
        {
            Products = new List<Product>{ // creating default list of products

                new Product{ID = 1, Name = "Product1", Quantity = 10, Price = 100.0, Rating = 5},
                new Product{ID = 2, Name = "Product2"},
                new Product{ID = 3, Name = "Product3"},
                new Product{ID = 4, Name = "Product4"}
            };
        }

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

        public long GetNextProductId()
        {
            lock (instanceLock)
            {
                return GetNextId();
            }
        }

        private long GetNextId()
        {
            return ++_highestIdUsed;
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