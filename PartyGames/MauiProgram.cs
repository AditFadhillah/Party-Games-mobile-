using Microsoft.Extensions.Logging;
using PartyGames.Services;
using PartyGames.ViewModels;
using PartyGames.Views;

namespace PartyGames;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register Services
		builder.Services.AddSingleton<IGameService, GameService>();
		builder.Services.AddSingleton<IScoreService, ScoreService>();
		builder.Services.AddSingleton(serviceProvider => new NavigationService(serviceProvider));
		builder.Services.AddSingleton<INavigationService>(serviceProvider => 
			serviceProvider.GetRequiredService<NavigationService>());

		// Register ViewModels
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<DiceGameViewModel>();
		builder.Services.AddSingleton<CoinFlipViewModel>();

		// Note: Don't register Views here - they're created dynamically by NavigationService
		// Only register MainPage which is created in App.xaml.cs
		builder.Services.AddSingleton<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

