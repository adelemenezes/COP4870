namespace Maui.eCommerce.Views;

public partial class ShoppingView : ContentPage
{
	public ShoppingView()
	{
		InitializeComponent();
	}
	private void ReturnToManagementMenuClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//MainPage");
	}

}