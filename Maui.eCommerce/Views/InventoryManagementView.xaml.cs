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
            
            var commerceService = new CommerceService(
                ProductServiceProxy.Current,
                new CartService(ProductServiceProxy.Current)
            );
            
            BindingContext = new InventoryManagementViewModel(commerceService);
        }

        private void ReturnToManagementMenuClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            if (BindingContext is InventoryManagementViewModel viewModel)
            {
                if (sender is Button button && button.BindingContext is Product product)
                {
                    viewModel.DeleteProduct(product);
                }
                else
                {
                    viewModel.DeleteProduct(); // Uses SelectedProduct
                }
            }
}      

        private void AddClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//AddProductView");
        }

        private void EditClicked(object sender, EventArgs e)
        {
            if (BindingContext is not InventoryManagementViewModel { SelectedProduct: { } product })
                return;

            Shell.Current.GoToAsync($"//EditProductView?productId={product.ID}");
        }

        private void ContentPage_NavigatingTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as InventoryManagementViewModel)?.RefreshProductList();
        }
    }
}