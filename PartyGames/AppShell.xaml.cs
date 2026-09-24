using PartyGames.Views;
using PartyGames.ViewModels;

namespace PartyGames;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	private void RegisterRoutes()
	{
		Routing.RegisterRoute("MainPage", typeof(MainPage));
		Routing.RegisterRoute("dice", typeof(DiceGamePage));
		Routing.RegisterRoute("coinflip", typeof(CoinFlipPage));
		Routing.RegisterRoute("blackjack", typeof(BlackjackPage));
	}
}
