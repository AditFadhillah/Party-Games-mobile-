namespace PartyGames.Features.TicTacToe;

public partial class TicTacToePage : ContentPage
{
	public TicTacToePage(TicTacToeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
