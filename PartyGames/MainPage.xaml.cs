using System.Windows.Input;
using PartyGames.Models;
using PartyGames.ViewModels;

namespace PartyGames;

public partial class MainPage : ContentPage
{
	private MainViewModel _viewModel;

	private static void Log(string message)
	{
		Console.WriteLine(message);
		File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] {message}\n");
	}

	public MainPage(MainViewModel viewModel)
	{
		Log("[MainPage.ctor] Constructor called");
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
		Log("[MainPage.ctor] BindingContext set");
	}

	private void OnDiceRollerClicked(object sender, EventArgs e)
	{
		Log("[MainPage] OnDiceRollerClicked called");
		try
		{
			var game = new Game { GameType = "dice", Name = "Dice Roller" };
			Log($"[MainPage] Created Game object with GameType: {game.GameType}");
			Log($"[MainPage] Executing SelectGameCommand");
			_viewModel.SelectGameCommand.Execute(game);
			Log($"[MainPage] SelectGameCommand executed");
		}
		catch (Exception ex)
		{
			Log($"[MainPage] OnDiceRollerClicked ERROR: {ex.Message}");
			Log($"[MainPage] Stack: {ex.StackTrace}");
		}
	}

	private void OnBlackjackClicked(object sender, EventArgs e)
	{
		Log("[MainPage] OnBlackjackClicked called");
		try
		{
			var game = new Game { GameType = "blackjack", Name = "Blackjack" };
			Log($"[MainPage] Created Game object with GameType: {game.GameType}");
			Log($"[MainPage] Executing SelectGameCommand");
			_viewModel.SelectGameCommand.Execute(game);
			Log($"[MainPage] SelectGameCommand executed");
		}
		catch (Exception ex)
		{
			Log($"[MainPage] OnBlackjackClicked ERROR: {ex.Message}");
			Log($"[MainPage] Stack: {ex.StackTrace}");
		}
	}

	private void OnCoinFlipClicked(object sender, EventArgs e)
	{
		Log("[MainPage] OnCoinFlipClicked called");
		try
		{
			var game = new Game { GameType = "coinflip", Name = "Coin Flip" };
			Log($"[MainPage] Created Game object with GameType: {game.GameType}");
			Log($"[MainPage] Executing SelectGameCommand");
			_viewModel.SelectGameCommand.Execute(game);
			Log($"[MainPage] SelectGameCommand executed");
		}
		catch (Exception ex)
		{
			Log($"[MainPage] OnCoinFlipClicked ERROR: {ex.Message}");
			Log($"[MainPage] Stack: {ex.StackTrace}");
		}
	}
}
