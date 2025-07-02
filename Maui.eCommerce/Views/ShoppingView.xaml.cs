using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;
using Library.eCommerce.Interfaces;

namespace Maui.eCommerce.Views
{
    public partial class ShoppingView : ContentPage
    {
        public ShoppingView()
        {
            InitializeComponent();
            var cartService = ProductServiceProxy.CartService;
            BindingContext = new ShoppingViewModel(cartService);
        }
        

        private void RemoveFromCartClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is long productId)
            {
                if (BindingContext is ShoppingViewModel vm)
                {
                    vm.RemoveFromCart(productId);
                }
            }
        }

        private void CheckoutClicked(object sender, EventArgs e)
        {
            if (BindingContext is ShoppingViewModel vm)
            {
                var receipt = vm.Checkout();
                DisplayAlert("Receipt", receipt, "OK");
            }

            Shell.Current.GoToAsync("//MainPage");
        }

        private void ReturnToManagementMenuClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }
    }
}