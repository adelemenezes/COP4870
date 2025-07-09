using System;
using System.Collections.Generic;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Library.eCommerce.Models
{
    public class CommerceSystem
    {
        public List<Product> Inventory { get; private set; }
        public List<Product> Cart { get; private set; }

        private readonly ProductServiceProxy _productService;

        const string DEFAULT_NAME = "NULL";
        const int DEFAULT_QUANTITY = 0;
        const double DEFAULT_PRICE = 0.00;
        const int DEFAULT_RATING = 0;

        public float TaxRate { get; set; } = 7.0f;

        public CommerceSystem()
        {
            _productService = ProductServiceProxy.Current;
            Inventory = _productService.Products ?? new List<Product>();
            Cart = new List<Product>();
        }

        public bool CreateItem()
        {
            Console.WriteLine("Please enter the following: ");

            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Quantity: ");
            string quantity = Console.ReadLine() ?? string.Empty;

            Console.Write("Price: ");
            string price = Console.ReadLine() ?? string.Empty;

            Console.Write("Rating (1-5): ");
            string rating = Console.ReadLine() ?? string.Empty;

            if (Product.TryCreate(name, quantity, price, rating, _productService.GetNextProductId(), out var product))
            {
                _productService.Products.Add(product);
                Console.WriteLine("Item created successfully");
                return true;
            }

            Console.WriteLine("Invalid input - item not created");
            return false;
        }

        public void Read(char whichList)
        {
            var list = whichList == 'i' ? Inventory : Cart;
            string listName = whichList == 'i' ? "Inventory" : "Shopping cart";

            if (list.Count == 0)
            {
                Console.WriteLine($"{listName} is empty.");
                return;
            }

            foreach (Product p in list)
            {
                Console.WriteLine(p);
            }
        }

        public bool UpdateItem()
        {
            Console.Write("Please give the ID of the item you would like updated: ");
            string input = Console.ReadLine() ?? string.Empty;

            if (!long.TryParse(input, out long id))
            {
                Console.WriteLine("Invalid entry.");
                return false;
            }

            foreach (Product p in Inventory)
            {
                if (p.ID == id)
                {
                    Console.WriteLine("\t\tEnter N: Update name.");
                    Console.WriteLine("\t\tEnter Q: Update quantity.");
                    Console.WriteLine("\t\tEnter P: Update price.");
                    Console.WriteLine("\t\tEnter R: Update rating.");
                    Console.WriteLine("\t\tEnter A: Update all.");

                    char selection = char.ToUpper(Console.ReadLine()?[0] ?? ' ');

                    switch (selection)
                    {
                        case 'N':
                            Console.Write("New name: ");
                            p.Name = ValidateName(Console.ReadLine() ?? string.Empty);
                            break;

                        case 'Q':
                            Console.Write("New quantity: ");
                            p.Quantity = ValidateQuantity(Console.ReadLine() ?? string.Empty);
                            break;

                        case 'P':
                            Console.Write("New price: ");
                            p.Price = ValidatePrice(Console.ReadLine() ?? string.Empty);
                            break;

                        case 'R':
                            Console.Write("New Rating: ");
                            p.Rating = ValidateRating(Console.ReadLine() ?? string.Empty);
                            break;

                        case 'A':
                            Console.Write("New name: ");
                            p.Name = ValidateName(Console.ReadLine() ?? string.Empty);

                            Console.Write("New Quantity: ");
                            p.Quantity = ValidateQuantity(Console.ReadLine() ?? string.Empty);

                            Console.Write("New price: ");
                            p.Price = ValidatePrice(Console.ReadLine() ?? string.Empty);

                            Console.Write("New Rating: ");
                            p.Rating = ValidateRating(Console.ReadLine() ?? string.Empty);
                            break;

                        default:
                            Console.WriteLine("Sorry! That choice is not an option.");
                            return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public bool UpdateItemAmount()
        {
            Console.Write("Please give ID of the item you would like updated: ");

            if (!long.TryParse(Console.ReadLine() ?? string.Empty, out long id))
            {
                Console.WriteLine("ERROR: Invalid ID.");
                return false;
            }

            Console.Write("New quantity: ");
            int tempQ = ValidateQuantity(Console.ReadLine() ?? string.Empty);

            Product? cartItem = Cart.Find(p => p.ID == id);
            Product? inventoryItem = Inventory.Find(p => p.ID == id);

            if (cartItem == null || inventoryItem == null)
            {
                Console.WriteLine("ERROR: Item not found.");
                return false;
            }

            int oldQuantity = cartItem.Quantity;

            if (oldQuantity > tempQ)
            {
                cartItem.Quantity = tempQ;
                inventoryItem.Quantity += oldQuantity - tempQ;

                if (cartItem.Quantity == 0)
                {
                    Cart.Remove(cartItem);
                }
            }
            else if (oldQuantity < tempQ)
            {
                if (tempQ - oldQuantity > inventoryItem.Quantity)
                {
                    Console.WriteLine("ERROR: Not enough items in inventory.");
                    return false;
                }

                inventoryItem.Quantity -= tempQ - oldQuantity;
                cartItem.Quantity = tempQ;
            }
            else
            {
                Console.WriteLine("No change in quantity.");
            }

            return true;
        }

        public bool RemoveItem()
        {
            Console.Write("Please give ID of the item you would like deleted: ");

            if (!long.TryParse(Console.ReadLine() ?? string.Empty, out long id))
            {
                return false;
            }

            bool foundInInventory = _productService.Products.RemoveAll(p => p?.ID == id) > 0;
            bool foundInCart = Cart.RemoveAll(p => p.ID == id) > 0;

            Console.WriteLine(foundInInventory
                ? $"Successfully removed ID: {id} from Inventory."
                : $"ID: {id} was not found in the inventory.");

            Console.WriteLine(foundInCart
                ? $"Successfully removed ID: {id} from Cart."
                : $"ID: {id} was not found in the shopping cart.");

            return foundInCart || foundInInventory;
        }

        public bool RemoveFromCart()
        {
            Console.Write("Please give the ID of the item you would like deleted: ");

            if (!long.TryParse(Console.ReadLine() ?? string.Empty, out long id))
            {
                Console.WriteLine("ERROR: Invalid ID.");
                return false;
            }

            Product? cartItem = Cart.Find(p => p.ID == id);
            Product? inventoryItem = Inventory.Find(p => p.ID == id);

            if (cartItem == null || inventoryItem == null)
            {
                Console.WriteLine("ERROR: Item not found.");
                return false;
            }

            inventoryItem.Quantity += cartItem.Quantity;
            Cart.Remove(cartItem);

            return true;
        }

        public bool AddToCart()
        {
            Console.Write("Please give ID of the item you would like to add to the cart: ");

            if (!long.TryParse(Console.ReadLine() ?? string.Empty, out long id) || id <= 0)
            {
                Console.WriteLine("ERROR: Invalid ID.");
                return false;
            }

            Product? inventoryItem = Inventory.Find(p => p.ID == id);

            if (inventoryItem == null)
            {
                Console.WriteLine("ERROR: Item could not be found.");
                return false;
            }

            if (inventoryItem.Quantity < 1)
            {
                Console.WriteLine("Sorry! That product is out of stock!");
                return false;
            }

            Product? cartItem = Cart.Find(p => p.ID == id);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new Product(inventoryItem) { Quantity = 1 };
                Cart.Add(cartItem);
            }

            inventoryItem.Quantity--;
            Console.WriteLine($"Successfully added {inventoryItem.Name} ID: {id} to the cart.");

            return true;
        }

        public void CheckOut()
        {
            double total = 0;
            int numberOfItems = 0;

            Console.WriteLine("\n********** CHECKOUT **********");

            foreach (Product p in Cart)
            {
                Console.WriteLine($"{p.Name}");
                Console.WriteLine($"\t${p.Price}");
                Console.WriteLine($"\tQuantity: {p.Quantity}");

                total += p.Price * p.Quantity;
                numberOfItems += p.Quantity;
            }

            Console.WriteLine("\n******************************");
            Console.WriteLine($"\nNumber of items: {numberOfItems}");
            Console.WriteLine($"Total Without tax: ${total:F2}");

            double tax = total * TaxRate / 100;

            Console.WriteLine($"Tax ({TaxRate:F1}%): ${tax:F2}");
            Console.WriteLine($"Total with tax: ${(total + tax):F2}");

            Console.WriteLine("\nThank you for your order!");
            Cart.Clear();
        }

        private string ValidateName(string name) =>
            string.IsNullOrWhiteSpace(name) ? DEFAULT_NAME : name;

        private int ValidateQuantity(string input) =>
            int.TryParse(input, out int temp) && temp >= 0 ? temp : DEFAULT_QUANTITY;

        private double ValidatePrice(string input) =>
            double.TryParse(input, out double temp) && temp >= 0 ? Math.Round(temp, 2) : DEFAULT_PRICE;

        private int ValidateRating(string input) =>
            int.TryParse(input, out int temp) && temp >= 0 && temp <= 5 ? temp : DEFAULT_RATING;
    }
}
