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

        private void OnDecreaseQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is long productId)
            {
                if (BindingContext is ShoppingViewModel vm)
                {
                    var item = vm.CartItems.FirstOrDefault(p => p.ID == productId);
                    if (item != null && item.Quantity > 1)
                    {
                        vm.UpdateCartItemQuantity(productId, item.Quantity - 1);
                    }
                    else
                    {
                        // At quantity 1, decrease will remove the item
                        vm.RemoveFromCart(productId);
                    }
                }
            }
        }

        private void OnIncreaseQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is long productId)
            {
                if (BindingContext is ShoppingViewModel vm)
                {
                    var item = vm.CartItems.FirstOrDefault(p => p.ID == productId);
                    if (item != null)
                    {
                        vm.UpdateCartItemQuantity(productId, item.Quantity + 1);
                    }
                }
            }
        }

        private void OnRemoveAllClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is long productId)
            {
                if (BindingContext is ShoppingViewModel vm)
                {
                    vm.RemoveFromCart(productId);
                }
            }
        }

    // Keep your existing CheckoutClicked and ReturnToManagementMenuClicked methods

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