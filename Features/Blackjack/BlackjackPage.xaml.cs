namespace PartyGames.Features.Blackjack;

public partial class BlackjackPage : ContentPage
{
	private const double ShakeDistance = 8;
	private const uint ShakeStepMs = 50;

	public BlackjackPage(BlackjackViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		viewModel.ShakeHandAsync = dealer => ShakeAsync(dealer ? DealerRow : PlayerRow);
		viewModel.ShowMoneyChangeAsync = ShowMoneyChangeAsync;
	}

	private async Task ShowMoneyChangeAsync(int change)
	{
		MoneyChangeLabel.Text = change > 0 ? $"+{change}" : change.ToString();
		MoneyChangeLabel.TextColor = change > 0 ? Color.FromArgb("#69F0AE") : Color.FromArgb("#FF5252");
		MoneyChangeLabel.TranslationY = 0;
		MoneyChangeLabel.Opacity = 1;

		await Task.WhenAll(
			MoneyChangeLabel.TranslateToAsync(0, 60, 1200, Easing.CubicOut),
			MoneyChangeLabel.FadeToAsync(0, 1200, Easing.CubicIn));
	}

	private static async Task ShakeAsync(VisualElement element)
	{
		for (int i = 0; i < 4; i++)
		{
			await element.TranslateToAsync(-ShakeDistance, 0, ShakeStepMs, Easing.Linear);
			await element.TranslateToAsync(ShakeDistance, 0, ShakeStepMs, Easing.Linear);
		}

		await element.TranslateToAsync(0, 0, 100, Easing.CubicOut);
	}
}
