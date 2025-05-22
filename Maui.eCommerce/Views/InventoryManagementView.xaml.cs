using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;

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
            (BindingContext as InventoryManagementViewModel)?.Delete();
        }

        private void AddClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ProductDetails");
        }

        private void ContentPage_NavigatingTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as InventoryManagementViewModel)?.RefreshProductList();
        }
    }
}