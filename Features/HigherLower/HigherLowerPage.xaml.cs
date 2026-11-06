namespace PartyGames.Features.HigherLower;

public partial class HigherLowerPage : ContentPage
{
	private const double ShakeDistance = 8;
	private const uint ShakeStepMs = 50;

	public HigherLowerPage(HigherLowerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		viewModel.DrawAnimationAsync = AnimateDrawAsync;
		viewModel.DiscardAnimationAsync = AnimateDiscardAsync;
	}

	private async Task AnimateDrawAsync()
	{
		ShowcaseView.TranslationX = StackView.X - ShowcaseView.X;
		ShowcaseView.TranslationY = StackView.Y - ShowcaseView.Y;
		await ShowcaseView.TranslateToAsync(0, 0, 450, Easing.CubicOut);
		await ShakeAsync(ShowcaseView);
	}

	private async Task AnimateDiscardAsync()
	{
		double x = DiscardPile.X - ShowcaseView.X;
		double y = DiscardPile.Y - ShowcaseView.Y;
		await ShowcaseView.TranslateToAsync(x, y, 450, Easing.CubicInOut);
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
