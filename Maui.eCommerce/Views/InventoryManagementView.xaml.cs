namespace Maui.eCommerce.Views;
using Maui.eCommerce.ViewModels;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Library.eCommerce.Interfaces;

public partial class InventoryManagementView : ContentPage
{
	public InventoryManagementView()
	{
		InitializeComponent();
		BindingContext = new InventoryManagementViewModel(); // set the binding context to the view model
	}

	private void ReturnToManagementMenuClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//MainPage");
	}

	private void DeleteClicked(object sender, EventArgs e){
		(BindingContext as InventoryManagementViewModel)?.Delete(); // delete the selected product
	}
	private void AddClicked(object sender, EventArgs e){
		Shell.Current.GoToAsync("//ProductDetails"); // go to the add product page
	}

	private void ContentPage_NavigatingTo(object sender, NavigatedToEventArgs e)
	{
		// this is called when the page is navigated to
		// we can put any code here that we want to run when the page is navigated to
		// for example, we can refresh the data in the view model
		(BindingContext as InventoryManagementViewModel)?.RefreshProductList(); // notify the view that the products have changed
																				// if as fails it returns null
																				// type coersion
		
	}
}