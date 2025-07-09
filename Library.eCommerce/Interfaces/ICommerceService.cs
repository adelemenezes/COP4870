using Library.eCommerce.Models;
using System;

namespace Library.eCommerce.Interfaces
{
    public interface ICommerceService : IProductService, ICartService
    {
        string ReadInventory();
        string ReadCart();

        bool CreateItem(string name, int quantity, double price, int rating);
        bool UpdateItem(long id, Action<Product> updateAction);
        bool UpdateItemAmount(long id, int newQuantity);
        bool RemoveItem(long id);

        string CheckOut();
    }
}
