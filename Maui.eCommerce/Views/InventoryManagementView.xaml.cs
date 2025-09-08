using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;
using Library.eCommerce.Models;

namespace Maui.eCommerce.Views
{
    public partial class InventoryManagementView : ContentPage
    {
        public InventoryManagementView()
        {
            InitializeComponent();
    
            var productService = ProductServiceProxy.Current;
            var cartService = ProductServiceProxy.CartService; 
            var commerceService = new CommerceService(productService, cartService);
            
            BindingContext = new InventoryManagementViewModel(commerceService);
        }

        private void ReturnToManagementMenuClicked(object sender, EventArgs e) 
            => Shell.Current.GoToAsync("//MainPage");

        private void AddClicked(object sender, EventArgs e) 
            => Shell.Current.GoToAsync("//AddProductView");

        private void EditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Product product)
            {
                Shell.Current.GoToAsync($"//EditProductView?productId={product.ID}");
            }
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Product product)
            {
                if (BindingContext is InventoryManagementViewModel vm)
                {
                    vm.DeleteProduct(product);
                }
            }
        }

        private void AddToCartClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Product product)
            {
                if (BindingContext is InventoryManagementViewModel vm)
                {
                    vm.AddToCart(product);
                }
            }
        }

        private void OnNavigatedTo(object sender, NavigatedToEventArgs e)
        {
            if (BindingContext is InventoryManagementViewModel vm)
            {
                vm.RefreshProductList();
            }
        }
    }
}