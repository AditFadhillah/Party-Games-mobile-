using PartyGames.Features.Blackjack;
using PartyGames.Features.CoinFlip;
using PartyGames.Features.DiceRoller;
using PartyGames.Features.Main;

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
