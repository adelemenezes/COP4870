using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ProductDetails : ContentPage
{
	public ProductDetails()
	{
		InitializeComponent();
		BindingContext = new ProductDetailsViewModel(); // set the binding context to the view model
	}

	private void ReturnToInventoryClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//InventoryManagement"); // go to the main page
	}
	private void ProductOKClicked(object sender, EventArgs e)
	{
		var name = (BindingContext as ProductDetailsViewModel).Name; // get the product from the BindingContext
		if (name != null)
		{
			ProductServiceProxy.Current.AddProduct(new Product() { Name = name }); // add the product to the service
		}
		Shell.Current.GoToAsync("//InventoryManagement"); // go to the main page
    }
}