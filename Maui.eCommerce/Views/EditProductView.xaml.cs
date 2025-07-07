using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;

namespace Maui.eCommerce.Views
{
    [QueryProperty(nameof(ProductId), "productId")]
    public partial class EditProductView : ContentPage
    {
        private long _productId;
        private ProductFormViewModel _viewModel;

        public long ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                LoadProductIfReady();
            }
        }

        public EditProductView()
        {
            InitializeComponent();
            var commerceService = new CommerceService(
                ProductServiceProxy.Current,
                new CartService(ProductServiceProxy.Current));

            _viewModel = new ProductFormViewModel(commerceService);
            BindingContext = _viewModel;
            LoadProductIfReady();
        }

        private void LoadProductIfReady()
        {
            if (_productId > 0 && _viewModel != null)
            {
                _viewModel.LoadProduct(_productId);
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
            => Shell.Current.GoToAsync("//InventoryManagement");

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (_viewModel.SaveProduct())
                Shell.Current.GoToAsync("//InventoryManagement");
        }
    }
}