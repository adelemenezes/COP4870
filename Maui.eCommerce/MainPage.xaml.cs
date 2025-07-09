using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce;

public partial class MainPage : ContentPage
// partial allows it to split the class into multiple files
// C++ doesn't need this becaue it has scope resolution
// MainPage is based off of content page, which is a class in the Maui framework
{

	public MainPage()
	{
		InitializeComponent(); // binding context
		BindingContext = new MainViewModel(); // of type object
											  // used to bind data to front end view
											  // this is not MVVM
	}



	public void InventoryManagementClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//InventoryManagement"); // go to the inventory management page
	}

	public void ShoppingClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//Shopping"); // // means next directory down
	}

	public void SettingsClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//Settings"); // go to the settings page
	}
}

