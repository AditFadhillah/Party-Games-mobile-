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

	private async void OnRollButtonClicked(object sender, EventArgs e)
	{
		if (_isAnimating)
			return;

		_isAnimating = true;
		
		// Start dice shake animation
		_ = AnimateDiceShake();
		
		// Let the command execute normally
		if (_viewModel.RollCommand is Command cmd && cmd.CanExecute(null))
		{
			cmd.Execute(null);
		}
		
		// Wait a bit before allowing next animation
		await Task.Delay(500);
		_isAnimating = false;
	}

	private async Task AnimateDiceShake()
	{
		const uint duration = 70;
		const double moveDistance = 10;

		// Shake 4 times for a total of ~560ms
		for (int i = 0; i < 4; i++)
		{
			// Shake left/right
			await Task.WhenAll(
				Die1Frame.TranslateTo(-moveDistance, 0, duration, Easing.Linear),
				Die2Frame.TranslateTo(moveDistance, 0, duration, Easing.Linear)
			);

			// Shake right/left
			await Task.WhenAll(
				Die1Frame.TranslateTo(moveDistance, 0, duration, Easing.Linear),
				Die2Frame.TranslateTo(-moveDistance, 0, duration, Easing.Linear)
			);
		}

		// Return to center
		await Task.WhenAll(
			Die1Frame.TranslateTo(0, 0, 100, Easing.CubicOut),
			Die2Frame.TranslateTo(0, 0, 100, Easing.CubicOut)
		);
	}
}
