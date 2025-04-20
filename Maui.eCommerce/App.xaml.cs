namespace Maui.eCommerce;

public partial class App : Application
{
	public App() // where call stack starts
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}