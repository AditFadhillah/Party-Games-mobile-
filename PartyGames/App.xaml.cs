using PartyGames.Views;
using PartyGames.ViewModels;

namespace PartyGames;

public partial class App : Application
{
	public App()
	{
		File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [App.ctor] START\n");
		InitializeComponent();
		File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [App.ctor] InitializeComponent completed\n");
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [CreateWindow] START\n");
		try
		{
			var services = IPlatformApplication.Current!.Services;
			File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [CreateWindow] Got services\n");
			
			var mainViewModel = services.GetRequiredService<MainViewModel>();
			File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [CreateWindow] Got MainViewModel\n");
			
			var mainPage = new MainPage(mainViewModel);
			File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [CreateWindow] Created MainPage\n");
			
			var navigationPage = new NavigationPage(mainPage);
			File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [CreateWindow] Created NavigationPage\n");
			
			var window = new Window(navigationPage);
			File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [CreateWindow] Created Window, returning\n");
			return window;
		}
		catch (Exception ex)
		{
			File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] [CreateWindow] ERROR: {ex.Message}\n{ex.StackTrace}\n");
			throw;
		}
	}
}
