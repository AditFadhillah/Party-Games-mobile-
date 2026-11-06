using PartyGames.Features.Main;


namespace PartyGames;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var services = IPlatformApplication.Current!.Services;
		var mainViewModel = services.GetRequiredService<MainViewModel>();
		var mainPage = new MainPage(mainViewModel);
		return new Window(new NavigationPage(mainPage));
	}
}
