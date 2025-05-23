using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;

namespace Maui.eCommerce.Views
{
    public partial class AddProductView : ContentPage
    {
        public AddProductView()
        {
            InitializeComponent();
            
            // Initialize services directly (no DI)
            var commerceService = new CommerceService(
                ProductServiceProxy.Current,
                new CartService(ProductServiceProxy.Current));
            
            BindingContext = new AddProductViewModel(commerceService);
        }

        // Keep your preferred event handlers
        private void ReturnToInventoryClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//InventoryManagement");
        }

        private void AddProductClicked(object sender, EventArgs e)
        {
            if (BindingContext is AddProductViewModel vm)
            {
                if (vm.AddProduct()) // Returns bool for success
                {
                    Shell.Current.GoToAsync("//InventoryManagement"); 
                }
            }
        }
    }
}