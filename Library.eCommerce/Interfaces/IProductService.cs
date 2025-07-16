using Library.eCommerce.Models;
using System.Collections.Generic;

namespace Library.eCommerce.Interfaces
{
    public interface IProductService
    {
        Product? GetProduct(long id);
        IEnumerable<Product> GetAllProducts();
        Product AddProduct(Product product);
        bool RemoveProduct(long id);
        long GetNextProductId();
        IEnumerable<Product> GetSortedProducts(SortOption sortBy);
    }
}
