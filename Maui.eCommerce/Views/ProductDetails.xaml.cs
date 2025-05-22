using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;

namespace Maui.eCommerce.Views
{
    public partial class ProductDetails : ContentPage
    {
        public ProductDetails()
        {
            InitializeComponent();
            
            // Initialize with your existing services
            var commerceService = new CommerceService(
                ProductServiceProxy.Current,
                new CartService(ProductServiceProxy.Current)
            );
            
            BindingContext = new ProductDetailsViewModel(commerceService);
        }

        private void ReturnToInventoryClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//InventoryManagement");
        }

        private void ProductOKClicked(object sender, EventArgs e)
        {
            if (BindingContext is ProductDetailsViewModel ProductsVM && ProductsVM.AddProduct())
            {
                Shell.Current.GoToAsync("//InventoryManagement");
            }
        }
    }
}