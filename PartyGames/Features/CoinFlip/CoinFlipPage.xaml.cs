using PartyGames.ViewModels;

namespace PartyGames.Features.CoinFlip;

public partial class CoinFlipPage : ContentPage
{
	public CoinFlipPage(CoinFlipViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
