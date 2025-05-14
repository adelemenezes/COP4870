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
}