using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;

namespace Maui.eCommerce.Views
{
    public partial class AddProductView : ContentPage
    {
       public AddProductView()
        {
            InitializeComponent();
            var commerceService = new CommerceService(
                ProductServiceProxy.Current,
                new CartService(ProductServiceProxy.Current));
            
            BindingContext = new ProductFormViewModel(commerceService);
        }   

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProductFormViewModel vm)
            {
                vm.ResetForm(); // Explicitly set to create mode
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
            => Shell.Current.GoToAsync("//InventoryManagement");

        private void OnAddClicked(object sender, EventArgs e)
        {
            if (BindingContext is ProductFormViewModel vm && vm.SaveProduct())
                Shell.Current.GoToAsync("//InventoryManagement");
        }
    }
}