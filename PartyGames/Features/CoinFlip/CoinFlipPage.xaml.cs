using PartyGames.ViewModels;

namespace PartyGames.Views;

public partial class CoinFlipPage : ContentPage
{
	public CoinFlipPage(CoinFlipViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
