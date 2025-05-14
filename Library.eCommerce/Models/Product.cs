using System;

namespace Library.eCommerce.Models
{
    public class Product
    {
        private string _name;
        private int _quantity;
        private double _price;
        private int _rating;
        
        public event Action<double> PriceChanged;

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? "NULL" : value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value >= 0 ? value : 0;
        }

        public long ID { get; set; }
        
        public double Price
        {
            get => _price;
            set
            {
                value = Math.Round(value >= 0 ? value : 0, 2);
                if (Math.Abs(_price - value) > 0.001)
                {
                    _price = value;
                    PriceChanged?.Invoke(_price);
                }
            }
        }

        public int Rating
        {
            get => _rating;
            set => _rating = (value >= 1 && value <= 5) ? value : 1;
        }

        public Product(string name, int quantity, double price, int rating, long key)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Rating = rating;
            ID = key;
        }

        public Product()
        {
            Name = "NULL";
            ID = 0;
            Quantity = 0;
            Price = 0;
            Rating = 1;
        }

        public Product(Product p)
        {
            Name = p.Name;
            Quantity = p.Quantity;
            Price = p.Price;
            Rating = p.Rating;
            ID = p.ID;
            
            p.PriceChanged += newPrice => Price = newPrice;
        }

        public static bool TryCreate(string name, string quantityStr, string priceStr, string ratingStr, long id, out Product product)
        {
            product = null;
            
            if (!int.TryParse(quantityStr, out int quantity)) return false;
            if (!double.TryParse(priceStr, out double price)) return false;
            if (!int.TryParse(ratingStr, out int rating)) return false;
            
            product = new Product(name, quantity, price, rating, id);
            return true;
        }

        /*public override string ToString()
        {
            return $"ID: {ID}\n\tName: {Name}\n\tQuantity: {Quantity}\n\tPrice: ${Price:F2}\n\tRating: {Rating}";
        }*/
        public string? Display{
            get
            {
                return $"ID: {ID}\tName: {Name}\tQuantity: {Quantity}\tPrice: ${Price:F2}\tRating: {Rating}";
            }

        }
        public override string ToString()
        {
            return Display ?? string.Empty; 
        }
    }
}