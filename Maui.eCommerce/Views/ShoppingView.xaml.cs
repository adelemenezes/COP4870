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

        private void OnNavigatedTo(object sender, NavigatedToEventArgs e)
		{
			if (BindingContext is ShoppingViewModel vm)
			{
				vm.RefreshCart();
			}
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
                // You would implement checkout logic here
                DisplayAlert("Checkout", "Checkout functionality would go here", "OK");
            }
        }

        private void ReturnToManagementMenuClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }
    }
}