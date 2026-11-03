using PartyGames.ViewModels;

namespace PartyGames.Views;

public partial class DiceGamePage : ContentPage
{
	private DiceGameViewModel _viewModel;
	private bool _isAnimating = false;

	public DiceGamePage(DiceGameViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	private async void OnRollButtonClicked(object? sender, EventArgs e)
	{
		if (_isAnimating)
			return;

		_isAnimating = true;
		
		// Start dice shake animation (command will execute via binding)
		await AnimateDiceShake();
		
		// Wait a bit before allowing next animation
		await Task.Delay(200);
		_isAnimating = false;
	}

	private async Task AnimateDiceShake()
	{
		const uint duration = 70;
		const double moveDistance = 10;

		// Shake the whole dice row 4 times, then return to center
		for (int i = 0; i < 4; i++)
		{
			await DiceRow.TranslateToAsync(-moveDistance, 0, duration, Easing.Linear);
			await DiceRow.TranslateToAsync(moveDistance, 0, duration, Easing.Linear);
		}

		await DiceRow.TranslateToAsync(0, 0, 100, Easing.CubicOut);
	}
}
