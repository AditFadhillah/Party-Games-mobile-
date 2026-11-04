using PartyGames.ViewModels;

namespace PartyGames.Features.CoinFlip;

public partial class CoinFlipPage : ContentPage
{
	private bool _isWiggling;

	public CoinFlipPage(CoinFlipViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		((CoinFlipViewModel)BindingContext).ClearHistory();
	}

	private async void OnCoinTapped(object? sender, TappedEventArgs e)
	{
		if (_isWiggling)
			return;

		_isWiggling = true;
		await WiggleCoin();
		_isWiggling = false;
	}

	private async Task WiggleCoin()
	{
		const uint duration = 70;
		const double moveDistance = 10;

		// Six shakes span roughly the same time as the spin frames
		for (int i = 0; i < 6; i++)
		{
			await Coin.TranslateToAsync(-moveDistance, 0, duration, Easing.Linear);
			await Coin.TranslateToAsync(moveDistance, 0, duration, Easing.Linear);
		}

		await Coin.TranslateToAsync(0, 0, 100, Easing.CubicOut);
	}
}
