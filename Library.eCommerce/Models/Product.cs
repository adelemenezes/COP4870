using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Library.eCommerce.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string _name = "NULL"; // default
        private int _quantity;
        private double _price;
        public long ID { get; set; }
        private int _rating;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                if (Math.Abs(_price - value) < 0.001) return;
                _price = Math.Round(value, 2);
                OnPropertyChanged();
            }
        }

        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating == value) return;
                _rating = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            
            p.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Name)) Name = p.Name;
                if (args.PropertyName == nameof(Price)) Price = p.Price;
                if (args.PropertyName == nameof(Rating)) Rating = p.Rating;
            };
        }

        public static bool TryCreate(string name, string quantityStr, string priceStr, string ratingStr, long id, out Product product)
        {
            product = default!;
            
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