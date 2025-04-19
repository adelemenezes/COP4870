using System;
using Library.eCommerce.Interfaces;
using Library.eCommerce.Models;

namespace ConsoleApp1.Services
{
    public class ConsoleCommerceService
    {
        private readonly ICommerceService _commerceService;

        public ConsoleCommerceService(ICommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        public void Run()
        {
            Console.WriteLine("Welcome to Adele's eCommerce Platform!");
            
            char choice;
            do {
                MainMenu();
                choice = GetMenuChoice();
                
                switch (choice) {
                    case 'I':
                        HandleInventoryMenu();
                        break;
                    case 'S':
                        HandleCartMenu();
                        break;
                    case 'M':
                        // Menu will redisplay automatically
                        break;
                    case 'Q':
                        HandleCheckout();
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command.");
                        break;
                }
            } while (choice != 'Q');
        }

        private void MainMenu()
        {
            Console.WriteLine("\n\t------MAIN MENU------");
            Console.WriteLine("\tEnter I: Access the inventory.");
            Console.WriteLine("\tEnter S: Access your shopping cart.");
            Console.WriteLine("\tEnter Q: Quit and check out.");
            Console.WriteLine("\tEnter M: See this menu again.");
        }

        private void HandleInventoryMenu()
        {
            char subChoice;
            do {
                InventoryMenu();
                subChoice = GetMenuChoice();
                
                switch(subChoice) {
                    case 'C':
                        HandleCreateItem();
                        break;
                    case 'R':
                        Console.WriteLine("\n" + _commerceService.ReadInventory());
                        break;
                    case 'U':
                        HandleUpdateItem();
                        break;
                    case 'D':
                        HandleDeleteItem();
                        break;
                    case 'A':
                        HandleAddToCart();
                        break;
                    case 'M':
                        Console.WriteLine("\nReturning to main menu...");
                        break;
                    case 'Q':
                        HandleCheckout();
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command.");
                        break;
                }
            } while (subChoice != 'M' && subChoice != 'Q');
        }

        private void InventoryMenu()
        {
            Console.WriteLine("\n\t------INVENTORY MENU------");
            Console.WriteLine("\tEnter C: Create new inventory item.");
            Console.WriteLine("\tEnter R: Read all inventory items.");
            Console.WriteLine("\tEnter U: Update an inventory item.");
            Console.WriteLine("\tEnter D: Delete an inventory item.");
            Console.WriteLine("\tEnter A: Add item from inventory to cart.");
            Console.WriteLine("\tEnter M: Return to Main Menu.");
            Console.WriteLine("\tEnter Q: Quit and check out.");
        }

        private void HandleCartMenu()
        {
            char subChoice;
            do {
                CartMenu();
                subChoice = GetMenuChoice();
                
                switch(subChoice) {
                    case 'R':
                        Console.WriteLine("\n" + _commerceService.ReadCart());
                        break;
                    case 'U':
                        HandleUpdateCartItem();
                        break;
                    case 'D':
                        HandleRemoveFromCart();
                        break;
                    case 'M':
                        Console.WriteLine("\nReturning to main menu...");
                        break;
                    case 'Q':
                        HandleCheckout();
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command.");
                        break;
                }
            } while (subChoice != 'M' && subChoice != 'Q');
        }

        private void CartMenu()
        {
            Console.WriteLine("\n\t------CART MENU------");
            Console.WriteLine("\tEnter R: Read all items in the cart.");
            Console.WriteLine("\tEnter U: Update an item amount in the cart.");
            Console.WriteLine("\tEnter D: Delete an item from Cart.");
            Console.WriteLine("\tEnter M: Return to Main Menu.");
            Console.WriteLine("\tEnter Q: Quit and check out.");
        }

        private void HandleCreateItem()
        {
            Console.WriteLine("\nPlease enter the following: ");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Quantity: ");
            int quantity = int.TryParse(Console.ReadLine(), out int q) ? q : 0;

            Console.Write("Price: ");
            double price = double.TryParse(Console.ReadLine(), out double p) ? p : 0;

            Console.Write("Rating (1-5): ");
            int rating = int.TryParse(Console.ReadLine(), out int r) ? r : 0;

            if (_commerceService.CreateItem(name, quantity, price, rating))
            {
                Console.WriteLine("\nItem created successfully");
            }
            else
            {
                Console.WriteLine("\nFailed to create item");
            }
        }

        private void HandleUpdateItem()
        {
            Console.Write("\nEnter ID of item to update: ");
            if (!long.TryParse(Console.ReadLine(), out long id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            Console.WriteLine("\tEnter N: Update name.");
            Console.WriteLine("\tEnter Q: Update quantity.");
            Console.WriteLine("\tEnter P: Update price.");
            Console.WriteLine("\tEnter R: Update rating.");
            Console.WriteLine("\tEnter A: Update all fields.");
            Console.Write("> ");
            
            var fieldChoice = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            bool success = fieldChoice switch {
                'N' => UpdateName(id),
                'Q' => UpdateQuantity(id),
                'P' => UpdatePrice(id),
                'R' => UpdateRating(id),
                'A' => UpdateAllFields(id),
                _ => false
            };

            Console.WriteLine(success ? "Update successful" : "Update failed");
        }

        private bool UpdateName(long id)
        {
            Console.Write("New name: ");
            string newName = Console.ReadLine() ?? string.Empty;
            return _commerceService.UpdateItem(id, p => p.Name = newName);
        }

        private bool UpdateQuantity(long id)
        {
            Console.Write("New quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int newQty)) return false;
            return _commerceService.UpdateItem(id, p => p.Quantity = newQty);
        }

        private bool UpdatePrice(long id)
        {
            Console.Write("New price: ");
            if (!double.TryParse(Console.ReadLine(), out double newPri)) return false;
            return _commerceService.UpdateItem(id, p => p.Price = newPri);
        }

        private bool UpdateRating(long id)
        {
            Console.Write("New rating: ");
            if (!int.TryParse(Console.ReadLine(), out int newRtg)) return false;
            return _commerceService.UpdateItem(id, p => p.Rating = newRtg);
        }

        private bool UpdateAllFields(long id)
        {
            bool success = true;

            Console.Write("New name: ");
            string newName = Console.ReadLine() ?? string.Empty;
            success &= _commerceService.UpdateItem(id, p => p.Name = newName);

            Console.Write("New quantity: ");
            if (int.TryParse(Console.ReadLine(), out int newQty))
            {
                success &= _commerceService.UpdateItem(id, p => p.Quantity = newQty);
            }
            else
            {
                success = false;
            }

            Console.Write("New price: ");
            if (double.TryParse(Console.ReadLine(), out double newPri))
            {
                success &= _commerceService.UpdateItem(id, p => p.Price = newPri);
            }
            else
            {
                success = false;
            }

            Console.Write("New rating: ");
            if (int.TryParse(Console.ReadLine(), out int newRtg))
            {
                success &= _commerceService.UpdateItem(id, p => p.Rating = newRtg);
            }
            else
            {
                success = false;
            }

            return success;
        }

        private void HandleAddToCart()
        {
            Console.Write("\nEnter ID of item to add to cart: ");
            if (!long.TryParse(Console.ReadLine(), out long id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            if (_commerceService.AddToCart(id))
            {
                Console.WriteLine("Item added to cart");
            }
            else
            {
                Console.WriteLine("Failed to add item to cart");
            }
        }

        private void HandleCheckout()
        {
            Console.WriteLine("\n" + _commerceService.Checkout());
        }

        private char GetMenuChoice()
        {
            Console.Write("\n> ");
            string input = Console.ReadLine() ?? string.Empty;
            return string.IsNullOrWhiteSpace(input) ? ' ' : char.ToUpper(input[0]);
        }

        private void HandleDeleteItem()
        {
            Console.Write("\nEnter ID of item to delete: ");
            if (!long.TryParse(Console.ReadLine(), out long id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            if (_commerceService.RemoveProduct(id))
            {
                Console.WriteLine($"Item {id} removed successfully");
            }
            else
            {
                Console.WriteLine($"Failed to remove item {id} (not found)");
            }
        }

        private void HandleUpdateCartItem()
        {
            Console.Write("\nEnter ID of cart item to update: ");
            if (!long.TryParse(Console.ReadLine(), out long id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            Console.Write("Enter new quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int newQuantity))
            {
                Console.WriteLine("Invalid quantity");
                return;
            }

            if (_commerceService.UpdateCartItemQuantity(id, newQuantity))
            {
                Console.WriteLine("Cart item updated successfully");
            }
            else
            {
                Console.WriteLine("Failed to update cart item (not enough stock or invalid ID)");
            }
        }

        private void HandleRemoveFromCart()
        {
            Console.Write("\nEnter ID of item to remove from cart: ");
            if (!long.TryParse(Console.ReadLine(), out long id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            if (_commerceService.RemoveFromCart(id))
            {
                Console.WriteLine($"Item {id} removed from cart");
            }
            else
            {
                Console.WriteLine($"Failed to remove item {id} from cart (not found)");
            }
        }
    }
}