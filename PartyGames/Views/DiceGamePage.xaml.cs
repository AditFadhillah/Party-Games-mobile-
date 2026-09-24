using PartyGames.ViewModels;

namespace PartyGames.Views;

public partial class DiceGamePage : ContentPage
{
	public DiceGamePage(DiceGameViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
