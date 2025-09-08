// Updated SettingsView.xaml.cs
using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;
using Library.eCommerce.Interfaces; // Add this line if ICartService is in this namespace


namespace Maui.eCommerce.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
            ICartService cartService = ProductServiceProxy.CartService;
            BindingContext = new SettingsViewModel(cartService);
        }
		
		private void GoBackClicked(object sender, EventArgs e) => Shell.Current.GoToAsync("//MainPage");
    }
}