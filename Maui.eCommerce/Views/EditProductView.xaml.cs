using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;

namespace Maui.eCommerce.Views
{
    [QueryProperty(nameof(ProductId), "productId")]
    public partial class EditProductView : ContentPage
    {
        public long ProductId { get; set; }

        public EditProductView()
        {
            InitializeComponent();
            var commerceService = new CommerceService(
                ProductServiceProxy.Current,
                new CartService(ProductServiceProxy.Current));
            
            BindingContext = new ProductFormViewModel(commerceService);
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            if (BindingContext is ProductFormViewModel vm && ProductId > 0)
                vm.LoadProduct(ProductId);
        }

        private void OnCancelClicked(object sender, EventArgs e)
            => Shell.Current.GoToAsync("//InventoryManagement");

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (BindingContext is ProductFormViewModel vm && vm.SaveProduct())
                Shell.Current.GoToAsync("//InventoryManagement");
        }
    }
}